# Gestion de parking

Ouvrir `P_OO-Parking/P_OO-Parking.sln` dans Visual Studio avec .NET 8.

Le tarif est de 1 CHF par heure, arrondi à deux décimales. Le programme utilise le temps réel et conserve les données pendant la session.

Pour exécuter les tests avec le SDK .NET 8 :

```bash
dotnet run --project Tests/Tests.csproj
```

Les résultats de la dernière exécution sont dans `Tests/Resultats.txt`.

Le journal conserve les heures saisies. Les durées manquantes et les appréciations personnelles restent à renseigner avec les informations réelles.
