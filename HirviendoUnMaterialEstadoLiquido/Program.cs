using System;
using System.Threading.Tasks;

double diametro = 0;
double cantidadGas = 0;
int velocidad = 0; // Posición de la válvula
string liquido = "";
double cantidadLiquido = 0;
int manubrio = 0;
double caudal = 0;
double PI = Math.PI;

Console.WriteLine("Simulación Hirviendo un Líquido\n");
Console.WriteLine("Elige una opción:");
Console.WriteLine("1. Tubería de 1/2 pulgada");
Console.WriteLine("2. Tubería de 1 pulgada");
Console.WriteLine("3. Tubería de 1 1/2 pulgada");
Console.WriteLine("4. Tubería de 2 pulgadas");
Console.WriteLine("5. Tubería de 2 1/2 pulgadas");
Console.WriteLine("6. Tubería de 3 pulgadas");
Console.Write("Elige una opción: ");

int opcionTuberia = int.Parse(Console.ReadLine());

switch (opcionTuberia)
{
	case 1:
		PedirRequerimientos(0.5);
		break;
	case 2:
		PedirRequerimientos(1);
		break;
	case 3:
		PedirRequerimientos(1.5);
		break;
	case 4:
		PedirRequerimientos(2);
		break;
	case 5:
		PedirRequerimientos(2.5);
		break;
	case 6:
		PedirRequerimientos(3);
		break;
	default:
		Console.WriteLine("Opción no válida. Saliendo...");
		return;
}

void PedirRequerimientos(double diametroSeleccionado)
{
	diametro = diametroSeleccionado;

	Console.Write("Ingrese cantidad del gas (en galones): ");
	cantidadGas = double.Parse(Console.ReadLine());

	Console.Write("Ingrese posición en la que se encuentra la válvula (1-4): ");
	velocidad = int.Parse(Console.ReadLine());

	Console.Write("Ingrese el líquido (agua o aceite): ");
	liquido = Console.ReadLine().ToLower();

	Console.Write("Ingrese la cantidad del líquido (en litros): ");
	cantidadLiquido = double.Parse(Console.ReadLine());

	Console.Write("Ingrese posición en la que se encuentra el manubrio de la estufa (0-5): ");
	manubrio = int.Parse(Console.ReadLine());

	IniciarSimulacion();
}

void IniciarSimulacion()
{
	bool liquidoHirviendo = false;

	double diametroAMetro = diametro * 0.0254;
	caudal = ((Math.PI * Math.Pow(diametroAMetro, 2)) / 4) * velocidad;
	double caudalEnGalonesPorSegundo = caudal * 264.172; //de m^3 a galones

	if (caudalEnGalonesPorSegundo <= 0)
	{
		Console.WriteLine("El caudal es cero o negativo, no hay flujo de gas.");
		return;
	}

	// Calcular tiempo total hasta agotar el gas
	double tiempoTotalSegundos = cantidadGas / caudalEnGalonesPorSegundo;
	int tiempoTotal = (int)Math.Ceiling(tiempoTotalSegundos);

	// Calcular tiempo hasta la ebullición del líquido
	double calorEspecifico = (liquido == "agua") ? 4.18 : 2.0; // J/g°C
	double densidadLiquido = (liquido == "agua") ? 1.0 : 0.92; // g/mL
	double puntoEbullicion = (liquido == "agua") ? 100.0 : 230.0; // °C
	double temperaturaInicial = 25.0; // Suponemos temperatura ambiente

	//cantidadLiquido esta en litros, se multiplica por 1000 para llevar a mililitros
	//y luego por densidadLiquido para llegar a gramos
	double masaLiquido = cantidadLiquido * 1000 * densidadLiquido; // en gramos

	//Q=m⋅c⋅ΔT
	double energiaNecesaria = masaLiquido * calorEspecifico * (puntoEbullicion - temperaturaInicial); // en J

	//Empieza en 5000 J, por cada potencia aumenta 8000
	double potenciaEstufa = 5000 + (manubrio * 8000); // J/s
	double tiempoHervorSegundos = energiaNecesaria / potenciaEstufa;
	int tiempoHervor = (int)Math.Ceiling(tiempoHervorSegundos);

	Console.Clear();
	Console.WriteLine("\n=== Simulación en curso ===");
	Console.WriteLine($"Tiempo estimado hasta que el gas se agote: {tiempoTotal} segundos");
	Console.WriteLine($"Tiempo estimado para que el {liquido} hierva: {tiempoHervor} segundos");

	int segundosTranscurridos = 0;

	while (cantidadGas > 0 && !liquidoHirviendo)
	{
		Console.WriteLine($"\n=== Segundo {segundosTranscurridos + 1} ===");

		cantidadGas -= caudalEnGalonesPorSegundo;
		if (cantidadGas < 0)
			cantidadGas = 0;

		Console.WriteLine($"Gas restante: {cantidadGas:F2} galones");
		Console.WriteLine($"Caudal actual: {caudalEnGalonesPorSegundo:F2} galones/segundo");
		MostrarBarraDeGas();

		if (segundosTranscurridos >= tiempoHervor)
		{
			Console.WriteLine($"\n¡El {liquido} ha comenzado a hervir!");
			Console.WriteLine("🔥 Deteniendo simulación...");
			break; // Se detiene la simulación
		}

		segundosTranscurridos++;
		System.Threading.Thread.Sleep(1000);
	}

	Console.WriteLine("\n¡Fin de la simulación!");

}

void MostrarBarraDeGas()
{
	int totalCaracteres = 30;
	int caracteresLlenos = (int)(cantidadGas * totalCaracteres / 10);

	Console.Write("[");
	for (int i = 0; i < totalCaracteres; i++)
	{
		if (i < caracteresLlenos)
			Console.Write("█");
		else
			Console.Write(" ");
	}
	Console.WriteLine("]");
}
