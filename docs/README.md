# hashfields

Selectively hash, drop, or keep fields from a flat file (e.g. CSV).

## Example

```console
$ echo "column1,column2,column3,column4
r1c1,r1c2,r1c3,r1c4
r2c1,r2c2,r2c3,r2c4
r3c1,r3c2,r3c3,r3c4
r4c1,r4c2,r4c3,r4c4" | hashfields --alg sha256 --drop column1 column3 --skip column2
column2,column4
r1c2,fb66e41761a74ea0c042e1c226c04fa2ce1a1334d7063d86230d17f33f109b68
r2c2,6051c006caee661a6ccb390b8cf7a43230c5cd7b54861f7306a598b612f924b9
r3c2,7e32c53b7729f5dce7ac54232b7f2d93d6c78ed19fc8d096b0fde948f513e9dc
r4c2,f8d0624d128daf97c61ec28f4396e8f14be2ca2940d18fdf33e939cda9bd1824
```

### Input CSV

Given this input CSV:

```csv
column1,column2,column3,column4
r1c1,r1c2,r1c3,r1c4
r2c1,r2c2,r2c3,r2c4
r3c1,r3c2,r3c3,r3c4
r4c1,r4c2,r4c3,r4c4
```

as a table:

| column1 | column2 | column3 | column4 |
| ------- | ------- | ------- | ------- |
| r1c1    | r1c2    | r1c3    | r1c4    |
| r2c1    | r2c2    | r2c3    | r2c4    |
| r3c1    | r3c2    | r3c3    | r3c4    |
| r4c1    | r4c2    | r4c3    | r4c4    |

### Configuration

These `hashfields` configuration options:

- **Hash algorithm:** `SHA256`
- **Drop:** `column1`, `column3`
- **Skip:** `column2`

### Output CSV

`hashfields` produces this CSV:

```csv
column2,column4
r1c2,fb66e41761a74ea0c042e1c226c04fa2ce1a1334d7063d86230d17f33f109b68
r2c2,6051c006caee661a6ccb390b8cf7a43230c5cd7b54861f7306a598b612f924b9
r3c2,7e32c53b7729f5dce7ac54232b7f2d93d6c78ed19fc8d096b0fde948f513e9dc
r4c2,f8d0624d128daf97c61ec28f4396e8f14be2ca2940d18fdf33e939cda9bd1824
```

as a table:

| column2 | column4                                                          |
| ------- | ---------------------------------------------------------------- |
| r1c2    | fb66e41761a74ea0c042e1c226c04fa2ce1a1334d7063d86230d17f33f109b68 |
| r2c2    | 6051c006caee661a6ccb390b8cf7a43230c5cd7b54861f7306a598b612f924b9 |
| r3c2    | 7e32c53b7729f5dce7ac54232b7f2d93d6c78ed19fc8d096b0fde948f513e9dc |
| r4c2    | f8d0624d128daf97c61ec28f4396e8f14be2ca2940d18fdf33e939cda9bd1824 |
