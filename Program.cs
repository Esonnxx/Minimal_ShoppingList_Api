using Microsoft.EntityFrameworkCore;
using MinimalShoppingListApi;
using MinimalShoppingListApi.Data;

var builder = WebApplication.CreateBuilder(args);  // 建立 WebApplicationBuilder 實例

builder.Services.AddEndpointsApiExplorer();  // 添加 API 資源探索服務
builder.Services.AddSwaggerGen();  // 添加 Swagger 生成服務
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("ShoppingListApi"));  // 添加 DbContext 服務，使用內存資料庫，名稱為 "ShoppingListApi"。

var app = builder.Build();  // 建立 WebApplication 實例


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
    app.UseSwagger();  // 在開發環境中啟用 Swagger 介面。
    app.UseSwaggerUI();  // 在開發環境中啟用 Swagger UI。
}

app.UseHttpsRedirection();  // 將 HTTP 請求重新導向到 HTTPS。
app.Run();  // 啟動應用程式的主要運行邏輯。
