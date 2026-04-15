using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.DTOs;
using ProductRecordSystem.Data;
using ProductRecordSystem.Models;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
	private readonly AppDbContext _context;
	public CustomerController(AppDbContext context) => _context = context;

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var customers = await _context.Customers
			.Select(c => new CustomerDto
			{
				Id = c.Id,
				FirstName = c.FirstName,
				LastName = c.LastName,
				Email = c.Email,
				Phone = c.Phone
			})
			.ToListAsync();
		return Ok(customers);
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetById(int id)
	{
		var c = await _context.Customers.FindAsync(id);
		if (c == null) return NotFound();
		return Ok(new CustomerDto
		{
			Id = c.Id,
			FirstName = c.FirstName,
			LastName = c.LastName,
			Email = c.Email,
			Phone = c.Phone
		});
	}

	[HttpGet("{id:int}/orders")]
	public async Task<IActionResult> GetOrders(int id)
	{
		var customer = await _context.Customers.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Id == id);
		if (customer == null) return NotFound();
		var orders = customer.Orders.Select(o => new OrderSummaryDto
		{
			Id = o.Id,
			Status = o.Status,
			OrderDate = o.OrderDate
		}).ToList();
		return Ok(orders);
	}

	[HttpPost]
	public async Task<IActionResult> Create(CreateCustomerDto dto)
	{
		var customer = new Customer
		{
			FirstName = dto.FirstName,
			LastName = dto.LastName,
			Email = dto.Email,
			Phone = dto.Phone
		};
		_context.Customers.Add(customer);
		await _context.SaveChangesAsync();
		return CreatedAtAction(nameof(GetById), new { id = customer.Id }, new CustomerDto
		{
			Id = customer.Id,
			FirstName = customer.FirstName,
			LastName = customer.LastName,
			Email = customer.Email,
			Phone = customer.Phone
		});
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, UpdateCustomerDto dto)
	{
		var customer = await _context.Customers.FindAsync(id);
		if (customer == null) return NotFound();
		customer.FirstName = dto.FirstName;
		customer.LastName = dto.LastName;
		customer.Email = dto.Email;
		customer.Phone = dto.Phone;
		await _context.SaveChangesAsync();
		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var customer = await _context.Customers.FindAsync(id);
		if (customer == null) return NotFound();
		_context.Customers.Remove(customer);
		await _context.SaveChangesAsync();
		return NoContent();
	}

	[HttpPost("bulk")]
	public async Task<IActionResult> BulkInsert(BulkCustomerInsertDto dto)
	{
		var customers = dto.Customers.Select(c => new Customer
		{
			FirstName = c.FirstName,
			LastName = c.LastName,
			Email = c.Email,
			Phone = c.Phone
		}).ToList();
		await _context.Customers.AddRangeAsync(customers);
		await _context.SaveChangesAsync();
		return Ok(new { inserted = customers.Count });
	}

	[HttpGet("with-orders")]
	public async Task<IActionResult> WithOrders()
	{
		var data = await _context.Customers
			.Include(c => c.Orders)
			.Select(c => new CustomerWithOrdersDto
			{
				Id = c.Id,
				FirstName = c.FirstName,
				LastName = c.LastName,
				Email = c.Email,
				Phone = c.Phone,
				Orders = c.Orders.Select(o => new OrderSummaryDto
				{
					Id = o.Id,
					Status = o.Status,
					OrderDate = o.OrderDate
				}).ToList()
			})
			.ToListAsync();
		return Ok(data);
	}

	[HttpGet("count")]
	public async Task<IActionResult> Count()
		=> Ok(new { totalCustomers = await _context.Customers.CountAsync() });

	[HttpGet("full-details")]
	public async Task<IActionResult> FullDetails()
	{
		var data = await _context.Customers
			.Include(c => c.Orders)
				.ThenInclude(o => o.OrderItems)
					.ThenInclude(oi => oi.Product)
			.Select(c => new CustomerFullDetailsDto
			{
				Id = c.Id,
				FirstName = c.FirstName,
				LastName = c.LastName,
				Email = c.Email,
				Phone = c.Phone,
				Orders = c.Orders.Select(o => new OrderWithItemsDto
				{
					Id = o.Id,
					Status = o.Status,
					OrderDate = o.OrderDate,
					Items = o.OrderItems.Select(oi => new OrderItemWithProductDto
					{
						ProductId = oi.ProductId,
						ProductName = oi.Product.Name,
						Quantity = oi.Quantity,
						UnitPrice = oi.UnitPrice,
						Product = new ProductSummaryDto
						{
							Id = oi.Product.Id,
							Name = oi.Product.Name,
							Price = oi.Product.Price,
							StockQty = oi.Product.StockQty
						}
					}).ToList()
				}).ToList()
			})
			.ToListAsync();
		return Ok(data);
	}
}
