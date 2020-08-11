using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioInventario.Aplicacion.Notificador;
using ServicioInventario.Datos;
using ServicioInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServicioInventario.Aplicacion.Tests
{
    [TestClass()]
    public class GestorInventarioTests
    {
        [TestMethod()]
        public void GestorInventarioTest()
        {
            //Arrange
            var mockRepositorio = new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);

            //Act
            new GestorInventario(mockRepositorio.Object, mockNotificador.Object);

            //Assert
            mockRepositorio.VerifyAll();
            mockNotificador.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void GestorInventarioTest_NullException()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);

            //Act
            new GestorInventario(null, mockNotificador.Object);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void GestorInventarioTest_NullException_2()
        {
            //Arrange
            var mockRepositorio = new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);

            //Act
            new GestorInventario(mockRepositorio.Object, null);
        }

        [TestMethod()]
        public void InsertarTest()
        {
            //Arrange
            var nuevoElemento = new ElementoInventario() { Nombre = "Test" };
            var mockRepositorio= new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);
            mockRepositorio.Setup(x => x.Crear(nuevoElemento));
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);

            //Act
            var target = new GestorInventario(mockRepositorio.Object, mockNotificador.Object);
            target.Insertar(nuevoElemento);

            //Assert
            mockRepositorio.VerifyAll();
            mockNotificador.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void InsertarTest_NullException()
        {
            //Arrange
            var mockRepositorio = new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);

            //Act
            var target = new GestorInventario(mockRepositorio.Object, mockNotificador.Object);
            target.Insertar(null);
        }

        [TestMethod()]
        public void SacarPorNombreTest()
        {
            //Arrange
            var elementos = new[] {
                new ElementoInventario() { Id = 1, Nombre = "Test", FechaDeCaducidadUTC = DateTime.UtcNow.AddDays(2) },
                new ElementoInventario() { Id = 2, Nombre = "Test", FechaDeCaducidadUTC = DateTime.UtcNow.AddDays(1) },
                new ElementoInventario() { Id = 3, Nombre = "Test", FechaDeCaducidadUTC = DateTime.UtcNow.AddDays(3) },
            };

            var mockRepositorio = new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);
            mockRepositorio.Setup(x => x.Consultar(It.IsAny<Func<IQueryable<ElementoInventario>, IQueryable<ElementoInventario>>>()))
                .Returns((Func<IQueryable<ElementoInventario>, IQueryable<ElementoInventario>> q) => q(elementos.AsQueryable()).ToList());
            mockRepositorio.Setup(x => x.Eliminar(2));
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);
            mockNotificador.Setup(x => x.Notificar(It.Is<string>(s => s.Contains("sacado")), elementos[1]));

            //Act
            var target = new GestorInventario(mockRepositorio.Object, mockNotificador.Object);
            var actual = target.SacarPorNombre("Test");

            //Assert
            Assert.AreEqual(elementos[1], actual);
            mockRepositorio.VerifyAll();
            mockNotificador.VerifyAll();
        }

        [TestMethod()]
        public void SacarPorNombreTest_2()
        {
            //Arrange
            var elementos = new[] {
                new ElementoInventario() { Id = 1, Nombre = "Test1", FechaDeCaducidadUTC = DateTime.UtcNow.AddDays(2) },
                new ElementoInventario() { Id = 2, Nombre = "Test2", FechaDeCaducidadUTC = DateTime.UtcNow.AddDays(1) },
                new ElementoInventario() { Id = 3, Nombre = "Test3", FechaDeCaducidadUTC = DateTime.UtcNow.AddDays(3) },
            };

            var mockRepositorio = new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);
            mockRepositorio.Setup(x => x.Consultar(It.IsAny<Func<IQueryable<ElementoInventario>, IQueryable<ElementoInventario>>>()))
                .Returns((Func<IQueryable<ElementoInventario>, IQueryable<ElementoInventario>> q) => q(elementos.AsQueryable()).ToList());
            mockRepositorio.Setup(x => x.Eliminar(3));
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);
            mockNotificador.Setup(x => x.Notificar(It.Is<string>(s => s.Contains("sacado")), elementos[2]));

            //Act
            var target = new GestorInventario(mockRepositorio.Object, mockNotificador.Object);
            var actual = target.SacarPorNombre("Test3");

            //Assert
            Assert.AreEqual(elementos[2], actual);
            mockRepositorio.VerifyAll();
            mockNotificador.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void SacarPorNombreTest_NullException()
        {
            //Arrange
            var mockRepositorio = new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);

            //Act
            var target = new GestorInventario(mockRepositorio.Object, mockNotificador.Object);
            target.SacarPorNombre(null);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod()]
        public void SacarPorNombreTest_NotFoundException()
        {
            //Arrange
            var mockRepositorio = new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);
            mockRepositorio.Setup(x => x.Consultar(It.IsAny<Func<IQueryable<ElementoInventario>, IQueryable<ElementoInventario>>>()))
                .Returns(() => new List<ElementoInventario>());
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);

            //Act
            var target = new GestorInventario(mockRepositorio.Object, mockNotificador.Object);
            target.SacarPorNombre("Test");
        }

        [TestMethod()]
        public void ObtenerCaducadosSinNotificarTest()
        {
            //Arrange
            var elementos = new[] {
                new ElementoInventario() { Id = 1, Nombre = "Test1", FechaDeCaducidadUTC = DateTime.UtcNow.AddDays(2) },
                new ElementoInventario() { Id = 2, Nombre = "Test2", FechaDeCaducidadUTC = DateTime.UtcNow.AddDays(-1) },
                new ElementoInventario() { Id = 3, Nombre = "Test3", FechaDeCaducidadUTC = DateTime.UtcNow.AddDays(-3) },
                new ElementoInventario() { Id = 3, Nombre = "Test3", FechaDeCaducidadUTC = DateTime.UtcNow.AddDays(-4), CaducidadNotificada = true },
            };

            var mockRepositorio = new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);
            mockRepositorio.Setup(x => x.Consultar(It.IsAny<Func<IQueryable<ElementoInventario>, IQueryable<ElementoInventario>>>()))
                .Returns((Func<IQueryable<ElementoInventario>, IQueryable<ElementoInventario>> q) => q(elementos.AsQueryable()).ToList());
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);

            //Act
            var target = new GestorInventario(mockRepositorio.Object, mockNotificador.Object);
            var actual = target.ObtenerCaducadosSinNotificar().ToList();

            //Assert
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(elementos[1], actual[0]);
            Assert.AreEqual(elementos[2], actual[1]);
            mockRepositorio.VerifyAll();
            mockNotificador.VerifyAll();
        }

        [TestMethod()]
        public void MarcarCaducidadNotificadaTest()
        {
            //Arrange
            var elementoPrueba = new ElementoInventario() { Nombre = "Test", CaducidadNotificada = false };

            var mockRepositorio = new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);
            mockRepositorio.Setup(x => x.Actualizar(3, It.IsAny<Action<ElementoInventario>>()))
                .Callback((int i, Action<ElementoInventario> a) => a(elementoPrueba));
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);

            //Act
            var target = new GestorInventario(mockRepositorio.Object, mockNotificador.Object);
            target.MarcarCaducidadNotificada(3);

            //Assert
            Assert.IsTrue(elementoPrueba.CaducidadNotificada);
            mockRepositorio.VerifyAll();
            mockNotificador.VerifyAll();
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod()]
        public void MarcarCaducidadNotificadaTest_NotFound()
        {
            //Arrange
            var mockRepositorio = new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);
            mockRepositorio.Setup(x => x.Actualizar(It.IsAny<int>(), It.IsAny<Action<ElementoInventario>>()))
                .Throws(new KeyNotFoundException());
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);

            //Act
            var target = new GestorInventario(mockRepositorio.Object, mockNotificador.Object);
            target.MarcarCaducidadNotificada(1);
        }

        [TestMethod()]
        public void DisposeTest()
        {
            //Arrange
            var mockRepositorio = new Mock<IRepositorio<ElementoInventario>>(MockBehavior.Strict);
            mockRepositorio.Setup(x => x.Dispose());
            var mockNotificador = new Mock<INotificador>(MockBehavior.Strict);

            //Act
            var target = new GestorInventario(mockRepositorio.Object, mockNotificador.Object);
            target.Dispose();

            //Assert
            mockRepositorio.VerifyAll();
            mockNotificador.VerifyAll();
        }
    }
}