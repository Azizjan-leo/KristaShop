using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities.Partners;
using Module.Common.Business.Actions;
using NUnit.Framework;

namespace Tests.Business.Common {
    //Tests naming convention: MethodName_Should_ExpectedBehavior_When_StateUnderTest
    public class StorehouseItemsComparatorTests {

        [Test]
        public void Should_ReturnExcessItems_When_DestinationItemsDoesNotContainItemFromSourceItems() {
            var sourceItems = new List<PartnerStorehouseItem> {
                new PartnerStorehouseItem {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("42"),
                    Amount = 9
                }
            };
            var destinationItems = new List<PartnerStorehouseItem>();

            var actual = CatalogItemsComparator.CheckForExcessAndDeficiency(sourceItems, destinationItems);

            actual.excess.Count.Should().Be(1);
            actual.deficiency.Count.Should().Be(0);
        }

        [Test]
        public void Should_SplitToSizes_When_SourceItemsContainSizeLines() {
            var sourceItems = new List<PartnerStorehouseItem> {
                new() {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("42-44-46"),
                    Amount = 3
                }
            };
            var destinationItems = new List<PartnerStorehouseItem>();

            var actual = CatalogItemsComparator.CheckForExcessAndDeficiency(sourceItems, destinationItems);
            var actualSizes = actual.excess.Select(x => x.Size.Value);
            var expectedSizes = new List<string> {"42", "44", "46"};

            actual.excess.Count.Should().Be(3);
            actualSizes.Should().BeEquivalentTo(expectedSizes);
            actual.excess.All(x => x.Amount == 3)
                .Should().Be(true, "beacuse item stores amount for line, when items is splitted it shoud store amount only for size");
            actual.deficiency.Count.Should().Be(0);
        }

        [Test]
        public void Should_ReturnDeficientItems_When_SourceItemsDoesNotContainItemFromDestinationItems() {
            var sourceItems = new List<PartnerStorehouseItem>();
            var destinationItems= new List<PartnerStorehouseItem> {
                new() {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("42"),
                    Amount = 9
                }
            };

            var actual = CatalogItemsComparator.CheckForExcessAndDeficiency(sourceItems, destinationItems);

            actual.deficiency.Count.Should().Be(1);
            actual.excess.Count.Should().Be(0);
        }

        [Test]
        public void Should_SplitToSizes_When_DestinationItemsContainSizeLines() {
            var sourceItems = new List<PartnerStorehouseItem>();
            var destinationItems = new List<PartnerStorehouseItem> {
                new() {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("42-44-46-48"),
                    Amount = 12
                }
            };

            var actual = CatalogItemsComparator.CheckForExcessAndDeficiency(sourceItems, destinationItems);
            var actualSizes = actual.deficiency.Select(x => x.Size.Value);
            var expectedSizes = new List<string> { "42", "44", "46", "48" };

            actual.deficiency.Count.Should().Be(4);
            actualSizes.Should().BeEquivalentTo(expectedSizes);
            actual.excess.All(x => x.Amount == 3)
                .Should().Be(true, "beacuse item stores amount for line, when items is splitted it shoud store amount only for size");
            actual.excess.Count.Should().Be(0);
        }

        [Test]
        public void Should_ReturnDeficientItems_When_SourceItemsContainItemWithLessAmountFromDestinationItems() {
            var sourceItems = new List<PartnerStorehouseItem> {
                new() {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("42"),
                    Amount = 7
                }
            };

            var destinationItems = new List<PartnerStorehouseItem> {
                new() {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("42"),
                    Amount = 9
                }
            };

            var actual = CatalogItemsComparator.CheckForExcessAndDeficiency(sourceItems, destinationItems);

            actual.deficiency.Count.Should().Be(1);
            actual.excess.Count.Should().Be(0);
        }

        [Test]
        public void Should_ReturnExcessItems_When_DestinationItemsContainItemWithLessAmountFromSourceItems() {
            var sourceItems = new List<PartnerStorehouseItem> {
                new() {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("42"),
                    Amount = 9
                }
            };

            var destinationItems = new List<PartnerStorehouseItem> {
                new() {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("42"),
                    Amount = 7
                }
            };

            var actual = CatalogItemsComparator.CheckForExcessAndDeficiency(sourceItems, destinationItems);

            actual.excess.Count.Should().Be(1);
            actual.deficiency.Count.Should().Be(0);
        }

        [Test]
        public void Should_ReturnNoExcessAndDeficiencyItems_When_SourceItemsAreLinesAndDestinationItemsAreNot() {
            var sourceItems = new List<PartnerStorehouseItem> {
                new() {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("42-44-46"),
                    Amount = 3
                }
            };

            var destinationItems = new List<PartnerStorehouseItem> {
                new() {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("42"),
                    Amount = 3
                },

                new() {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("44"),
                    Amount = 3
                },

                new() {
                    ModelId = 30,
                    Articul = "10-01",
                    ColorId = 15,
                    Size = new SizeValue("46"),
                    Amount = 3
                }
            };

            var actual = CatalogItemsComparator.CheckForExcessAndDeficiency(sourceItems, destinationItems);

            actual.excess.Count.Should().Be(0);
            actual.deficiency.Count.Should().Be(0);
        }
    }
}
