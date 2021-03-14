namespace TelRehber.Mongo
{
    //[MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class Stock
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        [System.ComponentModel.DataAnnotations.Key]
        public MongoDB.Bson.ObjectId Id { get; set; }
        public string Sku { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
    }
    public class SalesChannel
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        [System.ComponentModel.DataAnnotations.Key]
        public MongoDB.Bson.ObjectId Id { get; set; }
        public string SalesChannelId { get; set; }
        public System.Collections.Generic.List<string> Location { get; set; }
    }
    public class Lock
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        [System.ComponentModel.DataAnnotations.Key]
        public MongoDB.Bson.ObjectId Id { get; set; }
        public string SKU { get; set; }
        public string Location { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }
        public string TransactionId { get; set; }
        public string ReferenceId { get; set; }
    }
    public class Availability
    {
        public string SKU { get; set; }
        public System.Collections.Generic.List<LocationQuantity> LocationQuantities { get; set; }
    }
    public class LocationQuantity
    {
        public string Location { get; set; }
        public int Quantity { get; set; }
    }
}