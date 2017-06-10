using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;                

public class PasswordGenerator
{
    Queue<string> Results; // Warteschlange mit erzeugten Passwörtern

    bool UpperAZ; // true wenn Großbuchstaben benutzt werden sollen
    bool LowerAZ; // true wenn Kleinbuchstaben benutzt werden sollen
    bool Numbers; // true wenn Zahlen benutzt werden sollen

    int Min; // minimale Länge des Passworts
    int Max; // maximale Länge des Passworts

    const int MaxQueueSize = 30000000; // maximale Zahl von Passwörtern, die gleichzeitig in der Queue gespeichert werden dürfen (Schutz vor Speicherüberlauf)

    public PasswordGenerator(bool upperAZ, bool lowerAZ, bool numbers, int min, int max)
    {
        UpperAZ = upperAZ;
        LowerAZ = lowerAZ;
        Numbers = numbers;

        Min = min;
        Max = max;
        Results = new Queue<string>();

        // neuen Thread zur Passwortgenerierung erstellen und starten
        Thread Creator = new Thread(new ThreadStart(FillQueue));
        Creator.Start();
    }

    private void FillQueue()
    {
        // Warteschlange füllen
        for (int i = Min; i <= Max; i++)
        {
            SetLetter(i, 0, "");
        }
    }

    private void SetLetter(int length, int pos, string temp)
    {
        // aktuelle Position mit allen möglichen Zeichen füllen
        if (UpperAZ)
        {
            for (int i = 65; i <= 90; i++)
            {
                NextStep(length, pos, temp + (char)i);
            }
        }
        if (LowerAZ)
        {
            for (int i = 97; i <= 122; i++)
            {
                NextStep(length, pos, temp + (char)i);
            }
        }
        if (Numbers)
        {
            for (int i = 0; i <= 9; i++)
            {
                NextStep(length, pos, temp + i.ToString());
            }
        }
    }

    private void NextStep(int length, int pos, string temp)
    {
        // Funktion zum Abschließen eines Schrittes

        // ist die Warteschlange "voll", wird der Thread pausiert
        while (Results.Count > MaxQueueSize)
            Thread.Sleep(500);

        // ist noch nicht die Endposition im Passwort erreicht, wird SetLetters() mit der nächsten Position aufgerufen
        if (pos < length - 1)
            SetLetter(length, pos + 1, temp);
        else
            // ansonsten wird das aktuelle Passwort der Warteschlange hinzugefügt
            Results.Enqueue(temp);
    }

    public string Dequeue()
    {
        // liefert das unterste Element der Warteschlange
        if (Results.Count > 0)
            return Results.Dequeue();
        else
            return "";
    }
}

