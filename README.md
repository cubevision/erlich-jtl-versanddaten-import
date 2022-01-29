# Versanddaten importer for erlich bauhandel

This application will synchronize shipping data from email attachment exports and process and import these into JTL Wawi.

## Config

This project needs a config file with several options set in order to work properly. See below sample config.json

```json
{
  "Versand": [
    {
      "Spediteur": "GLS Germany",
      "VersandartId: 14
    },
    {
      "Spediteur": "WINNER Spedition",
      "VersandartId: 10
    }
  },
  "Imap": {
    "Host": "sample.host",
    "User": "sampleuser",
    "Password": "password",
    "Port: 993,
    "SSL": true
  },
  "Provider": [
    {
      "Sender": "foo@bar.de",
      "Subject": "GLS-Versendungen Erlich am",
      "Reader": "JTLVersandImport.Reader.HausfuxReader"
    },
    {
      "Sender": "bar@foo.de",
      "Subject": "Ihre Abfrage",
      "Reader": "JTLVersandImport.Reader.AmmonReader"
    }
  ],
  "DatabaseConnection": {
    "Server": "SERVER\\JTLWAWI",
    "Database": "eazybusiness",
    "User": "sa",
    "Password": "sa"
  }
}
```

By default the config will be read from the users home folder but can be changed in the CLI via `c --config` flag.
The config consists of several blocks, some of these are self explanatory and the rest will be explained below.

### Versand

The app needs to be configured with a Versandart to Id mapping.
The app can not know beforehand about the existing mappings in Wawi database and export labels.
In order to lookup the Ids the user needs to connect to the database and look these up.
