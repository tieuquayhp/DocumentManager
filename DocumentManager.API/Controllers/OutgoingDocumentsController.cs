// File: API/Controllers/OutgoingDocumentsController.cs
using AutoMapper;
using DocumentManager.API.DTOs;
using DocumentManager.API.Helpers;
using DocumentManager.DAL.Data;
using DocumentManager.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutgoingDocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OutgoingDocumentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private IQueryable<OutgoingDocument> GetFullDocumentsQuery()
        {
            return _context.OutgoingDocuments
                .Include(d => d.OutgoingDocumentType)
                .Include(d => d.OutgoingDocumentFormat)
                .Include(d => d.IssuingUnit)
                .Include(d => d.RelatedProject)
                .AsNoTracking();
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<OutgoingDocumentDto>>> GetOutgoingDocuments(
            [FromQuery] string? searchQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? limit = null)
        {
            var query = GetFullDocumentsQuery();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(d => d.OutgoingDocumentNumber.ToString().Contains(searchQuery) || d.DocumentContent.Contains(searchQuery));
            }

            query = query.OrderByDescending(d => d.ReleaseDate);

            if (limit.HasValue && limit > 0)
            {
                var limitedItems = await query.Take(limit.Value).ToListAsync();
                return Ok(_mapper.Map<List<OutgoingDocumentDto>>(limitedItems));
            }

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var dtos = _mapper.Map<List<OutgoingDocumentDto>>(items);
            return Ok(new PagedResult<OutgoingDocumentDto>(dtos, totalCount, pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OutgoingDocumentDto>> GetOutgoingDocument(int id)
        {
            var document = await GetFullDocumentsQuery().FirstOrDefaultAsync(d => d.Id == id);
            if (document == null) return NotFound();
            return Ok(_mapper.Map<OutgoingDocumentDto>(document));
        }

        [HttpPost]
        public async Task<ActionResult<OutgoingDocumentDto>> PostOutgoingDocument(OutgoingDocumentForCreationDto creationDto)
        {
            foreach (var groupId in creationDto.RecipientGroupIDs)
            {
                if (!await _context.RecipientGroups.AnyAsync(g => g.Id == groupId))
                    return BadRequest($"RecipientGroup với ID {groupId} không tồn tại.");
            }

            var document = _mapper.Map<OutgoingDocument>(creationDto);
            foreach (var groupId in creationDto.RecipientGroupIDs)
            {
                document.OutgoingDocumentRecipientGroups.Add(new OutgoingDocumentRecipientGroup { RecipientGroupID = groupId });
            }

            _context.OutgoingDocuments.Add(document);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<OutgoingDocumentDto>(document);
            return CreatedAtAction(nameof(GetOutgoingDocument), new { id = dto.ID }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOutgoingDocument(int id, OutgoingDocumentForUpdateDto updateDto)
        {
            var documentFromDb = await _context.OutgoingDocuments
                .Include(d => d.OutgoingDocumentRecipientGroups)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (documentFromDb == null) return NotFound();

            _mapper.Map(updateDto, documentFromDb);

            documentFromDb.OutgoingDocumentRecipientGroups.Clear();
            foreach (var groupId in updateDto.RecipientGroupIDs)
            {
                documentFromDb.OutgoingDocumentRecipientGroups.Add(new OutgoingDocumentRecipientGroup { RecipientGroupID = groupId });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOutgoingDocument(int id)
        {
            var document = await _context.OutgoingDocuments.FindAsync(id);
            if (document == null) return NotFound();
            _context.OutgoingDocuments.Remove(document);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}