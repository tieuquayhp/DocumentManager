// File: API/Controllers/IncomingDocumentsController.cs
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
    public class IncomingDocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public IncomingDocumentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private IQueryable<IncomingDocument> GetFullDocumentsQuery()
        {
            return _context.IncomingDocuments
                .Include(d => d.IssuingUnit)
                .Include(d => d.RelatedProject)
                .AsNoTracking();
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<IncomingDocumentDto>>> GetIncomingDocuments(
            [FromQuery] string? searchQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? limit = null)
        {
            var query = GetFullDocumentsQuery();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(d => d.IncomingDocumentNumber.ToString().Contains(searchQuery) || d.DocumentContent.Contains(searchQuery));
            }

            query = query.OrderByDescending(d => d.ReleaseDate);

            if (limit.HasValue && limit > 0)
            {
                var limitedItems = await query.Take(limit.Value).ToListAsync();
                return Ok(_mapper.Map<List<IncomingDocumentDto>>(limitedItems));
            }

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var dtos = _mapper.Map<List<IncomingDocumentDto>>(items);
            return Ok(new PagedResult<IncomingDocumentDto>(dtos, totalCount, pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IncomingDocumentDto>> GetIncomingDocument(int id)
        {
            var document = await GetFullDocumentsQuery().FirstOrDefaultAsync(d => d.Id == id);
            if (document == null) return NotFound();
            return Ok(_mapper.Map<IncomingDocumentDto>(document));
        }

        [HttpPost]
        public async Task<ActionResult<IncomingDocumentDto>> PostIncomingDocument(IncomingDocumentForCreationDto creationDto)
        {
            foreach (var groupId in creationDto.RecipientGroupIDs)
            {
                if (!await _context.RecipientGroups.AnyAsync(g => g.Id == groupId))
                    return BadRequest($"RecipientGroup với ID {groupId} không tồn tại.");
            }

            var document = _mapper.Map<IncomingDocument>(creationDto);
            foreach (var groupId in creationDto.RecipientGroupIDs)
            {
                document.IncomingDocumentRecipientGroups.Add(new IncomingDocumentRecipientGroup { RecipientGroupID = groupId });
            }

            _context.IncomingDocuments.Add(document);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<IncomingDocumentDto>(document);
            return CreatedAtAction(nameof(GetIncomingDocument), new { id = dto.ID }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncomingDocument(int id, IncomingDocumentForUpdateDto updateDto)
        {
            var documentFromDb = await _context.IncomingDocuments
                .Include(d => d.IncomingDocumentRecipientGroups)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (documentFromDb == null) return NotFound();

            _mapper.Map(updateDto, documentFromDb);

            documentFromDb.IncomingDocumentRecipientGroups.Clear();
            foreach (var groupId in updateDto.RecipientGroupIDs)
            {
                documentFromDb.IncomingDocumentRecipientGroups.Add(new IncomingDocumentRecipientGroup { RecipientGroupID = groupId });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncomingDocument(int id)
        {
            var document = await _context.IncomingDocuments.FindAsync(id);
            if (document == null) return NotFound();
            _context.IncomingDocuments.Remove(document);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}