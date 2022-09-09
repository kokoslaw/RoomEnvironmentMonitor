#include "DHT.h"
#include "SD.h"
#define DHT11_PIN 2
#define AnalogPIN A4

DHT dht;

void setup() 
{
  dht.setup(DHT11_PIN);
  Serial.begin(9600);
  Serial.setTimeout(10);
  pinMode(AnalogPIN, INPUT);
}

void loop() 
{
}

void serialEvent() 
{
  if (Serial.readString() == "SendData!\n")
  {
    String SerialData;
    int Temperature = dht.getTemperature();
    int Light = analogRead(AnalogPIN);
    int Humidity = dht.getHumidity();
    SerialData += (String)Temperature;
    SerialData += (String)Humidity;
    SerialData += (String)Light;
    Serial.println(SerialData);
  }
}
