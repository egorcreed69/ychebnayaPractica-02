using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ыы
{
    internal class Program
    {

    // Абстрактный класс "Функция в n-мерном пространстве"
    public abstract class NDimensionalFunction
    {
        public abstract double CalculateValue(double[] point);
        public abstract double[] CalculateGradient(double[] point);
    }

    // Класс "Линейная функция"
    public class LinearFunction : NDimensionalFunction
    {
        private readonly double[] coefficients;

        public LinearFunction(double[] coefficients)
        {
            this.coefficients = coefficients;
        }

        public override double CalculateValue(double[] point)
        {
            return coefficients.Zip(point, (coeff, coord) => coeff * coord).Sum();
        }

        public override double[] CalculateGradient(double[] point)
        {
            return coefficients;
        }
    }

    // Класс "Квадратичная функция"
    public class QuadraticFunction : NDimensionalFunction
    {
        private readonly double[] coefficients;

        public QuadraticFunction(double[] coefficients)
        {
            this.coefficients = coefficients;
        }

        public override double CalculateValue(double[] point)
        {
            return coefficients.Zip(point, (coeff, coord) => coeff * coord * coord).Sum();
        }

        public override double[] CalculateGradient(double[] point)
        {
            return coefficients.Select((coeff, index) => 2 * coeff * point[index]).ToArray();
        }
    }

    // Класс "Множество точек в n-мерном пространстве"
    public class PointSet
    {
        private readonly List<Tuple<NDimensionalFunction, double>> inequalities;

        public PointSet(List<Tuple<NDimensionalFunction, double>> inequalities)
        {
            this.inequalities = inequalities;
        }

        public bool IsPointInSet(double[] point)
        {
            return inequalities.All(inequality => inequality.Item1.CalculateValue(point) <= inequality.Item2);
        }

        public bool IsPointOnBoundary(double[] point)
        {
            return inequalities.Any(inequality => Math.Abs(inequality.Item1.CalculateValue(point) - inequality.Item2) < double.Epsilon);
        }
    }
    static void Main(string[] args)
        {
            //создание линейной функции
            var linearFunc = new LinearFunction(new double[] { 2, 3 });
            //создание квадратичной функции
            var quadraticFunc = new QuadraticFunction(new double[] { 2, 3, -1, -2 });

            // Создание множества точек с неравенствами: f(x) <= 10 и f(x) <= -5
            var pointSet = new PointSet(new List<Tuple<NDimensionalFunction, double>>
            {
                Tuple.Create(linearFunc, 10.0),
                Tuple.Create(quadraticFunc, -5.0)
                    //это не будет работать так как в 91 строке недопустимый аргумент
            });

            // Проверка принадлежности точки множеству
            double[] point = { 1, 2 };
            bool isInSet = pointSet.IsPointInSet(point);
            Console.WriteLine($"Point is in set: {isInSet}");

            // Проверка, лежит ли точка на границе множества
            bool isOnBoundary = pointSet.IsPointOnBoundary(point);
            Console.WriteLine($"Point is on boundary: {isOnBoundary}");
            Console.ReadKey();
        }
    }
   
}
