#include <EEPROM.h>
#define xdirpin 11
#define xsteppin 10
#define ydirpin 5
#define ysteppin 4
#define zdirpin 7
#define zsteppin 8
#define xenable 6
#define yenable 3
#define zenable 2
#define m0 9
String sdeger;
String degisken;
float deger = 0;
float xcurrent = 0;
float ycurrent = 0;
float zcurrent = 0;
float xkonum = 0;
float ykonum = 0;
float zkonum = 0;
int feedrate = 0;
boolean state = false;
int spindle = 0;

void setup() {
  Serial.begin(9600);
  Serial.setTimeout(100);
  pinMode(xenable, OUTPUT);
  pinMode(yenable, OUTPUT);
  pinMode(zenable, OUTPUT);
  pinMode(xdirpin, OUTPUT);
  pinMode(xsteppin, OUTPUT);
  pinMode(ydirpin, OUTPUT);
  pinMode(ysteppin, OUTPUT);
  pinMode(zdirpin, OUTPUT);
  pinMode(zsteppin, OUTPUT);
  pinMode(m0, OUTPUT);
  digitalWrite(m0, HIGH);
  digitalWrite(xenable, HIGH);
  digitalWrite(yenable, HIGH);
  digitalWrite(zenable, HIGH);
  EEPROM.get(10, xkonum);
  Serial.println(xkonum);
  EEPROM.get(15, ykonum);
  Serial.println(ykonum);

  EEPROM.get(20, zkonum);
  Serial.println(zkonum);

}

void okuma()  {
  String serialread = Serial.readString();
  degisken = serialread.substring(0, 1);
  sdeger = serialread.substring(1, 5);
  deger = sdeger.toFloat();
}

void loop() {

  state = false;
  while (Serial.available() == 0)  {
  }
  okuma();
  if (degisken == "G" && deger == 00) {
    feedrate = 20;
    state = true;
    Serial.println("D");
  }
  else if (degisken == "G" && deger == 01) {
    feedrate = 20;
    state = true;
    Serial.println("D");
  }
  else if (degisken == "X")  {
    state = true;
    xcurrent = deger;
    if (xkonum < xcurrent) {
      digitalWrite(xdirpin, HIGH);
      digitalWrite(xenable, LOW);
      float a = ((xcurrent - xkonum) * 800);
      long t = a;
      for (long i = 0; i < t; i++) {
        digitalWrite(xsteppin, HIGH);
        delayMicroseconds(feedrate);
        digitalWrite(xsteppin, LOW);
        delayMicroseconds(feedrate);
      }
    }
    else if (xkonum > xcurrent) {
      digitalWrite(xdirpin, LOW);
      digitalWrite(xenable, LOW);
      float a = ((xkonum - xcurrent) * 800);
      long t = a;
      for (long i = 0; i < t ; i++) {
        digitalWrite(xsteppin, HIGH);
        delayMicroseconds(feedrate);
        digitalWrite(xsteppin, LOW);
        delayMicroseconds(feedrate);
      }
    }
    digitalWrite(xenable, HIGH);
    xkonum = xcurrent;
    EEPROM.put(10, xkonum);
    Serial.println("D");
    while (Serial.available() == 0)  {
    }
    String serialread = Serial.readString();
    sdeger = serialread.substring(0, 4);
    deger = sdeger.toFloat();
    ycurrent = deger;
    if (ykonum < ycurrent) {
      digitalWrite(ydirpin, HIGH);
      digitalWrite(yenable, LOW);
      float a = ((ycurrent - ykonum) * 800);
      long t = a;
      for (long i = 0; i < t; i++) {
        digitalWrite(ysteppin, HIGH);
        delayMicroseconds(feedrate);
        digitalWrite(ysteppin, LOW);
        delayMicroseconds(feedrate);
      }
    }
    else if (ykonum > ycurrent) {
      digitalWrite(ydirpin, LOW);
      digitalWrite(yenable, LOW);
      float a = ((ykonum - ycurrent) * 800);
      long t = a;
      for (long i = 0; i < t; i++) {
        digitalWrite(ysteppin, HIGH);
        delayMicroseconds(feedrate);
        digitalWrite(ysteppin, LOW);
        delayMicroseconds(feedrate);
      }
    }
    digitalWrite(yenable, HIGH);
    ykonum = ycurrent;
    EEPROM.put(15, ykonum);
    Serial.println("D");
  }
  else if (degisken == "Z") {
    state = true;
    zcurrent = deger;
    if (zkonum > zcurrent) {
      digitalWrite(zdirpin, HIGH);
      long a = ((zkonum - zcurrent) * 800);
      digitalWrite(zenable, LOW);
      for (long i = 0; i < a; i++) {
        digitalWrite(zsteppin, HIGH);
        delayMicroseconds(feedrate);
        digitalWrite(zsteppin, LOW);
        delayMicroseconds(feedrate);
      }
    }
    else if (zkonum < zcurrent) {
      digitalWrite(zdirpin, LOW);
      long a = ((zcurrent - zkonum) * 800);
      digitalWrite(zenable, LOW);
      for (long i = 0; i < a; i++) {
        digitalWrite(zsteppin, HIGH);
        delayMicroseconds(feedrate);
        digitalWrite(zsteppin, LOW);
        delayMicroseconds(feedrate);
      }
    }
    digitalWrite(zenable, HIGH);
    zkonum = zcurrent;
    EEPROM.put(20, zkonum);
    Serial.println("D");
  }
  else if (degisken == "M" && deger == 03) {
    spindle = 1;
    state = true;
    Serial.println("D");
  }
  else if (degisken == "M" && deger == 05) {
    digitalWrite(xenable, HIGH);
    digitalWrite(yenable, HIGH);
    digitalWrite(zenable, HIGH);
    spindle = 0;
  }
  else if (degisken == "G" && deger == 04)  {
    state = true;
    Serial.println("D");
    while (Serial.available() == 0)  {
    }
    okuma();
    if (degisken == "P") {
      float d = deger;
      delay(d * 1000);
      Serial.println("D");
    }
  }
  else if (degisken == "R") {
    state = true;
    if (EEPROM.get(10, xkonum) != 0) {
      Serial.print("In reset: ");
      Serial.println(xkonum);

      long  a = xkonum * 800;
      Serial.println(a);
      digitalWrite(xdirpin, LOW);
      digitalWrite(xenable, LOW);
      for (long i = 0; i < a; i++) {
        digitalWrite(xsteppin, HIGH);
        delayMicroseconds(20);
        digitalWrite(xsteppin, LOW);
        delayMicroseconds(20);
      }
      digitalWrite(xenable, HIGH);
      xkonum = 0;
      EEPROM.put(10, xkonum);
    }
    if ( EEPROM.get(15, ykonum) != 0) {
      long a = ykonum * 800;
      digitalWrite(ydirpin, LOW);
      digitalWrite(yenable, LOW);
      for (long i = 0; i < a; i++) {
        digitalWrite(ysteppin, HIGH);
        delayMicroseconds(20);
        digitalWrite(ysteppin, LOW);
        delayMicroseconds(20);
      }
      digitalWrite(yenable, HIGH);
      ykonum = 0;
      EEPROM.put(15, ykonum);
    }
    if ( EEPROM.get(20, zkonum) < 0) {
      long a = -zkonum * 800;
      digitalWrite(zdirpin, LOW);
      digitalWrite(zenable, LOW);
      for (long i = 0; i < a; i++) {
        digitalWrite(zsteppin, HIGH);
        delayMicroseconds(20);
        digitalWrite(zsteppin, LOW);
        delayMicroseconds(20);
      }
      digitalWrite(zenable, HIGH);
      zkonum = 0;
      EEPROM.put(20, zkonum);
    }
    if (EEPROM.get(20, zkonum) > 0) {
      long a = zkonum * 800;
      digitalWrite(zdirpin, HIGH);
      digitalWrite(zenable, LOW);
      for (long i = 0; i < a; i++) {
        digitalWrite(zsteppin, HIGH);
        delayMicroseconds(20);
        digitalWrite(zsteppin, LOW);
        delayMicroseconds(20);
      }
      digitalWrite(zenable, HIGH);
      zkonum = 0;
      EEPROM.put(20, zkonum);
    }
    Serial.println("D");
  }
  else if (state == false)
    Serial.println("D");
}
