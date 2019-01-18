namespace StripeTests
{
    using System.Collections.Generic;
    using Stripe;
    using Stripe.Infrastructure.Extensions;
    using Xunit;

    public class AccountCreateOptionsTest : BaseStripeTest
    {
        [Fact]
        public void SerializeAdditionalOwnersProperly()
        {
            var options = new AccountCreateOptions
            {
                LegalEntity = new AccountLegalEntityOptions
                {
                    AdditionalOwners = new List<AccountAdditionalOwner>
                    {
                        new AccountAdditionalOwner
                        {
                            Dob = new AccountDobOptions
                            {
                                Day = 1,
                                Month = 1,
                                Year = 1980,
                            },
                            FirstName = "John",
                            LastName = "Doe",
                            Verification = new AccountVerificationOptions
                            {
                                DocumentBackId = "file_234",
                                DocumentId = "file_123",
                            },
                        },
                        new AccountAdditionalOwner
                        {
                            Address = new AddressOptions
                            {
                                State = "CA",
                                City = "City",
                                Line1 = "Line1",
                                Line2 = "Line2",
                                PostalCode = "90210",
                                Country = "US",
                            },
                            FirstName = "Jenny",
                            LastName = "Rosen",
                        }
                    },
                },
            };

            Assert.Equal(
                "legal_entity[additional_owners][0][dob][day]=1&" +
                "legal_entity[additional_owners][0][dob][month]=1&" +
                "legal_entity[additional_owners][0][dob][year]=1980&" +
                "legal_entity[additional_owners][0][first_name]=John&" +
                "legal_entity[additional_owners][0][last_name]=Doe&" +
                "legal_entity[additional_owners][0][verification][document_back]=file_234&" +
                "legal_entity[additional_owners][0][verification][document]=file_123&" +
                "legal_entity[additional_owners][1][address][city]=City&" +
                "legal_entity[additional_owners][1][address][country]=US&" +
                "legal_entity[additional_owners][1][address][line1]=Line1&" +
                "legal_entity[additional_owners][1][address][line2]=Line2&" +
                "legal_entity[additional_owners][1][address][postal_code]=90210&" +
                "legal_entity[additional_owners][1][address][state]=CA&" +
                "legal_entity[additional_owners][1][first_name]=Jenny&" +
                "legal_entity[additional_owners][1][last_name]=Rosen",
                options.ToQueryString());
        }

        [Fact]
        public void SerializeEmptyAdditionalOwnersProperly()
        {
            var options = new AccountCreateOptions
            {
                LegalEntity = new AccountLegalEntityOptions
                {
                    AdditionalOwners = new List<AccountAdditionalOwner>()
                },
            };

            Assert.Equal("legal_entity[additional_owners]=", options.ToQueryString());
        }
    }
}
