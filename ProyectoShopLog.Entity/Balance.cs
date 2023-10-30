using ProyectoShopLog.Entity;

namespace ProyectoShopLog.Entity;

public class Balance
{
    public List<Gasto> Ingresos;
    public List<Gasto> Gastos;

    public Balance(List<Gasto> gastos, List<Gasto> ingresos)
    {
        Ingresos = ingresos;
        Gastos = gastos;
    }

    public double TotalIngresos
    {
        get
        {
            return Ingresos.Aggregate(0d, (acumulador, ingreso) => acumulador + (double)ingreso.Monto);
        }
    }

    public double TotalGastos
    {
        get
        {
            return Gastos.Aggregate(0d, (acumulador, gasto) => acumulador + (double)gasto.Monto);
        }
    }

    public double SaldoResultante
    {
        get
        {
            return TotalIngresos - TotalGastos;
        }
    }
}