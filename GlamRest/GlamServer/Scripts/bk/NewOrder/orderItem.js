function orderItemViewModel(product) {
    this.self = this;

    this.ProductID = product.ProductID;
    this.Quantity = 0;
    this.DiscountInShekels = 0;
    this.ProductName = product.ProductName;
    this.Price = product.Price;

    this.updateProduct = function (product) {
        self.ProductID = product.ProductID;
        self.Quantity = 0;
        self.DiscountInShekels = 0;
        self.ProductName = product.ProductName;
        self.Price = product.Price;
    }
}