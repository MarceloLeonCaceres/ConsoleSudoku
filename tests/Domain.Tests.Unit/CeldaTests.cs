using Domain.Entities;

namespace Domain.Tests.Unit;

public class CeldaTests
{
    [Theory]
    // Esquinas de la matriz
    [InlineData(0, 0, 0)] // Cuadrante superior izquierdo
    [InlineData(0, 8, 2)] // Cuadrante superior derecho
    [InlineData(8, 0, 6)] // Cuadrante inferior izquierdo
    [InlineData(8, 8, 8)] // Cuadrante inferior derecho
    // Centro y casos medios
    [InlineData(4, 4, 4)] // Cuadrante central
    [InlineData(2, 5, 1)] // Fila 2 (Cuad 0 en Y), Col 5 (Cuad 1 en X) -> Cuadrante 1
    [InlineData(5, 2, 3)] // Fila 5 (Cuad 1 en Y), Col 2 (Cuad 0 en X) -> Cuadrante 3
    public void Constructor_CoordenadasValidas_CalculaCuadranteCorrecto(int x, int y, int zEsperado)
    {
        // Act
        var celda = new Celda(x, y);

        // Assert
        Assert.Equal(x, celda.X);
        Assert.Equal(y, celda.Y);
        Assert.Equal(zEsperado, celda.Z);
    }

    [Theory]
    [InlineData(-1, 5)]
    [InlineData(9, 5)]
    public void Constructor_FilaInvalida_LanzaArgumentOutOfRangeException(int x, int y)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Celda(x, y));
        Assert.Equal("x", exception.ParamName);
    }

    [Theory]
    [InlineData(5, -1)]
    [InlineData(5, 9)]
    public void Constructor_ColumnaInvalida_LanzaArgumentOutOfRangeException(int x, int y)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Celda(x, y));
        Assert.Equal("y", exception.ParamName);
    }

    [Fact]
    public void Records_ConMismasCoordenadas_SeEvaluanComoIguales()
    {
        // Arrange
        var celda1 = new Celda(3, 4);
        var celda2 = new Celda(3, 4);

        // Act & Assert
        Assert.Equal(celda1, celda2); // En un class fallaría por referencia, en un record pasa por valor
    }
}
