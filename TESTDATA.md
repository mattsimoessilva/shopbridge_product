## Test Data for Product Service API


### POST/api/Product
```
{
  "name": "Wireless Bluetooth Headphones",
  "shortDescription": "Compact wireless headphones with noise cancellation",
  "fullDescription": "These Bluetooth headphones deliver up to 20 hours of playback, premium sound quality, and adaptive noise cancellation. Comes with a carrying case and fast-charging USB-C cable.",
  "price": 129.99,
  "discountPrice": 99.99,
  "isActive": true,
  "isFeatured": true,
  "sku": "WH-1000XM4",
  "stockQuantity": 150,
  "minimumStockThreshold": 10,
  "allowBackorder": false,
  "brand": "Sony",
  "category": "Electronics",
  "tags": "headphones, bluetooth, audio, wireless",
  "imageUrl": "https://example.com/images/headphones.jpg",
  "ThumbnailUrl": "https://example.com/images/headphones-thumb.jpg",
  "seoTitle": "Sony Wireless Bluetooth Headphones - Noise Cancelling",
  "seoDescription": "Shop the best wireless noise cancelling headphones with long battery life and premium sound quality.",
  "slug": "sony-wireless-bluetooth-headphones"
}
```


### POST/api/ProductVariant
```
{
  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "variantName": "Wireless Headphones - Black Edition",
  "color": "Black",
  "size": "Standard",
  "additionalPrice": 20.00,
  "stockQuantity": 75,
  "imageUrl": "https://example.com/images/headphones-black.jpg",
  "isActive": true
}
```





