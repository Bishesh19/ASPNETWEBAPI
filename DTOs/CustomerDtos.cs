using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WeatherAPI.DTOs;

public class CreateCustomerDto
{
	[Required]
	public string FirstName { get; set; } = string.Empty;

	[Required]
	public string LastName { get; set; } = string.Empty;

	[Required]
	[EmailAddress]
	public string Email { get; set; } = string.Empty;

	public string Phone { get; set; } = string.Empty;
}

public class UpdateCustomerDto
{
	[Required]
	public string FirstName { get; set; } = string.Empty;

	[Required]
	public string LastName { get; set; } = string.Empty;

	[Required]
	[EmailAddress]
	public string Email { get; set; } = string.Empty;

	public string Phone { get; set; } = string.Empty;
}

public class CustomerDto
{
	public int Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
}

public class CustomerSummaryDto
{
	public int Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
}

public class BulkCustomerInsertDto
{
	public List<CreateCustomerDto> Customers { get; set; } = new();
}

public class CustomerWithOrdersDto
{
	public int Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public List<OrderSummaryDto> Orders { get; set; } = new();
}

public class CustomerFullDetailsDto
{
	public int Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public List<OrderWithItemsDto> Orders { get; set; } = new();
}

public class OrderSummaryDto
{
	public int Id { get; set; }
	public string Status { get; set; } = string.Empty;
	public System.DateTime OrderDate { get; set; }
}

public class OrderWithItemsDto
{
	public int Id { get; set; }
	public string Status { get; set; } = string.Empty;
	public System.DateTime OrderDate { get; set; }
	public List<OrderItemWithProductDto> Items { get; set; } = new();
}

public class OrderItemWithProductDto
{
	public int ProductId { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public decimal UnitPrice { get; set; }
	public ProductSummaryDto Product { get; set; } = new();
}


