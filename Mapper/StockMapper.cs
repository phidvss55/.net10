using webapi.Dtos.Stock;
using webapi.Models;

namespace webapi.Mapper;

public static class StockMapper
{
    public static StockDto ToStockDto(this Stock stockModel)
    {
        return new StockDto(
            stockModel.Id,
            stockModel.Symbol,
            stockModel.CompanyName,
            stockModel.Price,
            stockModel.LastDiv,
            stockModel.Industry,
            stockModel.MarketCap,
            stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
        );
    }

    public static Stock ToStockFromRequest(this CreateStockRequest stockDto)
    {
        return new Stock
        {
            Symbol = stockDto.Symbol,
            CompanyName = stockDto.CompanyName,
            Price = stockDto.Price,
            LastDiv = stockDto.LastDiv,
            Industry = stockDto.Industry,
            MarketCap = stockDto.MarketCap
        };
    }
    
    public static Stock ToStockFromFMP(this FMPStockDto fmpStock)
    {
        return new Stock
        {
            Symbol = fmpStock.symbol,
            CompanyName = fmpStock.companyName,
            Price = (decimal)fmpStock.price,
            LastDiv = (decimal)fmpStock.lastDiv,
            Industry = fmpStock.industry,
            MarketCap = fmpStock.mktCap
        };
    }

}