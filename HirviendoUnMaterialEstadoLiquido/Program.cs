//Variables
double diametro = 0;
double cantidadGas = 0;
int velocidad = 0; //Posicion de la valvula


double caudal = 0;
double PI = Math.PI;



// Conversión de pulgadas a metros
double diametroAMetro = diametro * 0.0254;  // 1 pulgada = 0.0254 metros

// Cálculo del caudal usando el diámetro en metros
caudal = ((Math.PI * Math.Pow(diametroAMetro, 2)) / 4) * velocidad;


Console.WriteLine(caudal);



