﻿using System;
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Moq;
using Streetwood.Core.Domain.Abstract.Repositories;
using Streetwood.Core.Extensions;
using Streetwood.Infrastructure.Dto;
using Streetwood.Infrastructure.Services.Implementations.Queries;
using Xunit;

namespace Streetwood.Infrastructure.Tests.QueryServices
{
    public class ProductCategoryDiscountTests
    {
        private readonly Mock<IProductCategoryDiscountRepository> categoryDiscountRepository;
        private readonly Mock<IProductCategoryRepository> productCategoryRepository;
        private readonly Mock<IDiscountCategoryRepository> discountCategoryRepository;
        private readonly Mock<IMapper> mapper;

        public ProductCategoryDiscountTests()
        {
            categoryDiscountRepository = new Mock<IProductCategoryDiscountRepository>();
            productCategoryRepository = new Mock<IProductCategoryRepository>();
            discountCategoryRepository = new Mock<IDiscountCategoryRepository>();
            mapper = new Mock<IMapper>();
        }

        [Fact]
        public void ApplyDiscountsToProducts_ForEmptyDiscounts_ReturnsNull()
        {
            // arrange
            var products = new List<ProductDto>
            {
                new ProductDto()
            };
            var discounts = new List<ProductCategoryDiscountDto>();
            var sut = new ProductCategoryDiscountQueryService(categoryDiscountRepository.Object,
                productCategoryRepository.Object, discountCategoryRepository.Object, mapper.Object);

            // act
            var result = sut.ApplyDiscountsToProducts(products, discounts);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public void ApplyDiscountToProducts_ReturnValidPairs()
        {
            // arrange
            var testData = PrepareTestData();
            var products = testData.Item1;
            var discounts = testData.Item2;

            var expectedValue = 8;
            var sut = new ProductCategoryDiscountQueryService(categoryDiscountRepository.Object,
                productCategoryRepository.Object, discountCategoryRepository.Object, mapper.Object);

            // act
            var result = sut.ApplyDiscountsToProducts(products, discounts);

            // assert
            result.Count.Should().Be(expectedValue);
        }

        private (List<ProductDto>, List<ProductCategoryDiscountDto>) PrepareTestData()
        {
            var categoryIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            var products = new List<ProductDto>();
            var discounts = new List<ProductCategoryDiscountDto>();

            // we add 2 real products and last fake, not belong to any categories
            foreach (var id in categoryIds)
            {
                products.Add(new ProductDto
                {
                    Id = 10.GetRandom(),
                    ProductCategoryId = id
                });
                products.Add(new ProductDto
                {
                    Id = 10.GetRandom(),
                    ProductCategoryId = id
                });
                products.Add(new ProductDto
                {
                    Id = 10.GetRandom(),
                    ProductCategoryId = Guid.NewGuid()
                });
            }

            // and here 2 real discounts and one fake 
            discounts.Add(new ProductCategoryDiscountDto
            {
                Id = Guid.NewGuid(),
                CategoryIds = new List<Guid>
                {
                    categoryIds[0],
                    categoryIds[1]
                }
            });

            discounts.Add(new ProductCategoryDiscountDto
            {
                Id = Guid.NewGuid(),
                CategoryIds = new List<Guid>
                {
                    categoryIds[2],
                    categoryIds[3]
                }
            });

            discounts.Add(new ProductCategoryDiscountDto
            {
                Id = Guid.NewGuid(),
                CategoryIds = new List<Guid>
                {
                    Guid.NewGuid(),
                    Guid.NewGuid()
                }
            });

            return (products, discounts);
        }
    }
}