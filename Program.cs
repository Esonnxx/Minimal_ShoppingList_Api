using Microsoft.EntityFrameworkCore;
using MinimalShoppingListApi;
using MinimalShoppingListApi.Data;

var builder = WebApplication.CreateBuilder(args);  // �إ� WebApplicationBuilder ���

builder.Services.AddEndpointsApiExplorer();  // �K�[ API �귽�����A��
builder.Services.AddSwaggerGen();  // �K�[ Swagger �ͦ��A��
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("ShoppingListApi"));  // �K�[ DbContext �A�ȡA�ϥΤ��s��Ʈw�A�W�٬� "ShoppingListApi"�C

var app = builder.Build();  // �إ� WebApplication ���


app.MapGet("/shoppingList", async (ApiDbContext db) => 
await db.Groceries.ToListAsync());

app.MapPost("/shoppingList", async (Grocery grocery, ApiDbContext db) =>
{
    db.Groceries.Add(grocery);
    await db.SaveChangesAsync();
    return Results.Created($"/shoppinglist/{grocery.Id}", grocery);
});
app.MapGet("/shoppinglist/{id}", async (int id, ApiDbContext db) =>
{
    var targetGrocery = await db.Groceries.FindAsync(id);
    return targetGrocery != null? Results.Ok(targetGrocery): Results.NotFound();
});
app.MapDelete("/shoppinglist/{id}", async (int id, ApiDbContext db) =>
{
    var targetGrocery = await db.Groceries.FindAsync(id);
    if (targetGrocery != null)
    {
        db.Groceries.Remove(targetGrocery);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});
app.MapPut("/shoppinglist/{id}", async (int id, Grocery grocery ,ApiDbContext db) =>
{
    var targetItem = await db.Groceries.FindAsync(id);
    if(targetItem != null)
    {
        targetItem.Name = grocery.Name;
        targetItem.Purchased = grocery.Purchased;
        await db.SaveChangesAsync();
        return Results.Ok(targetItem);
    }
    return Results.NotFound();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // �b�}�o���Ҥ��ҥ� Swagger �����C
    app.UseSwaggerUI();  // �b�}�o���Ҥ��ҥ� Swagger UI�C
}

app.UseHttpsRedirection();  // �N HTTP �ШD���s�ɦV�� HTTPS�C
app.Run();  // �Ұ����ε{�����D�n�B���޿�C
