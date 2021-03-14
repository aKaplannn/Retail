using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TelRehber.Mongo;
namespace TelRehber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetailController : ControllerBase
    {
        private readonly MongoDBRepositoryFactory _mongoDB;
        public RetailController(MongoDBRepositoryFactory mongoDBRepositoryFactory)
        {
            _mongoDB = mongoDBRepositoryFactory;
        }
        [HttpPost]
        [Route("SetStocks")]
        public bool SetStocks(List<Stock> stockList)
        {
            try
            {
                IRepository<Stock> stockRepository = _mongoDB.Build<Stock>("Stock");
                stockList?.ForEach((s) =>
                    {
                        Stock stock = stockRepository.FindOneAsync(f => f.Sku == s.Sku && f.Location == s.Location).Result;
                        if (stock is Stock)
                        {
                            Dictionary<string, object> updateFields = new Dictionary<string, object>();
                            updateFields.Add("Quantity", s.Quantity);
                            stockRepository.UpdateOneAsync(p => p.Id == stock.Id, updateFields);
                        }
                        else stockRepository.InsertOneAsync(s);
                    });
            }
            catch { return false; }
            return true;
        }
        [HttpGet]
        public List<Stock> GetStocks()
        {
            return _mongoDB.Build<Stock>("Stock").FindAsync(_ => true).Result;
        }
        [HttpPost]
        [Route("SetSalesChannel")]
        public bool SetSalesChannel(SalesChannel salesChannel)
        {
            IRepository<SalesChannel> salesChannelRepository = _mongoDB.Build<SalesChannel>("SalesChannel");
            salesChannelRepository.DeleteManyAsync(a => a.SalesChannelId == salesChannel.SalesChannelId);
            return salesChannelRepository.InsertOneAsync(salesChannel).IsCompletedSuccessfully;
        }
        [HttpGet]
        [Route("GetSalesChannels")]
        public List<SalesChannel> GetSalesChannels()
        {
            return _mongoDB.Build<SalesChannel>("SalesChannel").FindAsync(_ => true).Result;
        }

        [HttpPost]
        [Route("GetAvailability")]
        public List<Availability> GetAvailability(string salesChannelId, List<string> skus)
        {
            IRepository<SalesChannel> salesChannelRepository = _mongoDB.Build<SalesChannel>("SalesChannel");
            IRepository<Stock> stockRepository = _mongoDB.Build<Stock>("Stock");
            IRepository<Lock> lockRepository = _mongoDB.Build<Lock>("Lock");
            List<Availability> availabilityList = new List<Availability>();

            SalesChannel salesChannel = salesChannelRepository.FindOneAsync(s => s.SalesChannelId == salesChannelId).Result;

            if (salesChannel is null)
                return availabilityList;

            List<Stock> stockList = stockRepository.FindAsync(s => s.Quantity > 0 && salesChannel.Location.Contains(s.Location) && skus.Contains(s.Sku)).Result;            

            foreach (string sku in skus)
            {
                List<LocationQuantity> locationQuantityList = new List<LocationQuantity>();
                foreach (var stock in stockList.Where(a => a.Sku == sku))
                {
                    Lock Lock = lockRepository.FindOneAsync(f => stock.Sku == f.SKU && stock.Location == f.Location).Result;
                    int locks = Lock is Lock ? Lock.Amount : 0;
                    locationQuantityList.Add(new LocationQuantity() { Location = stock.Location, Quantity = stock.Quantity - locks });                    
                }
                if (locationQuantityList.Count > 0) 
                {
                    Availability availability = new Availability();
                    availability.SKU = sku;
                    availability.LocationQuantities = locationQuantityList;
                    availabilityList.Add(availability);
                }
            }

            return availabilityList;
        }
        [HttpPost]
        [Route("SetLock")]
        public bool SetLock(Lock Lock)
        {
            IRepository<Lock> lockRepository = _mongoDB.Build<Lock>("Lock");
            lockRepository.DeleteManyAsync(a => a.Location == Lock.Location && a.SKU == Lock.SKU);
            return lockRepository.InsertOneAsync(Lock).IsCompleted;
        }
    }
}