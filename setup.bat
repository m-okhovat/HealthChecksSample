dotnet tool install --global dotnet-ef

dotnet tool update --global dotnet-ef

dotnet ef database update --project "GloboTicket.Services.Discount"

dotnet ef database update --project "GloboTicket.Services.EventCatalog"

dotnet ef database update --project "GloboTicket.Services.Marketing"

dotnet ef database update --project "GloboTicket.Services.Order"

dotnet ef database update --project "GloboTicket.Services.ShoppingBasket"