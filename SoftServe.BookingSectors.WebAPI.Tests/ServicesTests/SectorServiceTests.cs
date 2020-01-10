﻿using AutoMapper;
using Moq;
using NUnit.Framework;
using SoftServe.BookingSectors.WebAPI.BLL.DTO;
using SoftServe.BookingSectors.WebAPI.BLL.Mapping;
using SoftServe.BookingSectors.WebAPI.BLL.Services;
using SoftServe.BookingSectors.WebAPI.BLL.Services.Interfaces;
using SoftServe.BookingSectors.WebAPI.DAL.Models;
using SoftServe.BookingSectors.WebAPI.DAL.Repositories;
using SoftServe.BookingSectors.WebAPI.DAL.UnitOfWork;
using SoftServe.BookingSectors.WebAPI.Tests.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftServe.BookingSectors.WebAPI.Tests.ServicesTests
{
    [TestFixture]
    class SectorServiceTests
    {
        readonly ISectorService sectorService;
        readonly Mock<IUnitOfWork> unitOfWork;
        readonly Mock<IBaseRepository<Sector>> repository;
        List<Sector> sectorsContext;

        public SectorServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SectorProfile>();
            });
            repository = new Mock<IBaseRepository<Sector>>();
            unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(u => u.SectorRepository).Returns(repository.Object);
            sectorService = new SectorService(unitOfWork.Object, config.CreateMapper());
        }

        [SetUp]
        public void SetUp()
        {
            sectorsContext = SectorData.Sectors;
        }

        [Test]
        public async Task GetAllSectors_SectorDataSectors_AllReturned()
        {
            //Arrange
            repository.Setup(r => r.GetAllEntitiesAsync()).ReturnsAsync(sectorsContext);
            //Act
            var results = await sectorService.GetSectorsAsync() as List<SectorDTO>;
            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(sectorsContext.Count, results.Count);
        }
    }
}