Documentation:
https://stripe.com/docs
Video Tutorial 
https://www.youtube.com/watch?v=e-whXipfRvg
3 different js files with React Stripe.js
-Not too hard to implement with plenty of resources out there teaching you how to use Stripe
-payment
-Completion form
-Checkout form

## Usage...

- Csharp
```cs
// imports for payment.js file 
import {loadStripe} from "@stripe/stripe.js";


StripeConfiguration.ApiKey = "KEY";

// Create a payment intent to start a purchase flow.
var options = new PaymentIntentCreateOptions
{
    Amount = 2000,
    Currency = "usd",
    Description = "My first payment",
};
var service = new PaymentIntentService();
var paymentIntent = service.Create(options);

// Complete the payment using a test card.
var confirmOptions = new PaymentIntentConfirmOptions
{
    PaymentMethod = "pm_card_mastercard",
};
service.Confirm(paymentIntent.Id, confirmOptions);
```

