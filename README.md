# SARIF Converter

[![NuGet Version](https://img.shields.io/nuget/v/milkyware-sarif-converter)](https://www.nuget.org/packages/milkyware-sarif-converter)
[![NuGet Downloads](https://img.shields.io/nuget/dt/milkyware-sarif-converter)](https://www.nuget.org/packages/milkyware-sarif-converter)
[![Latest release](https://img.shields.io/github/v/release/milkyware/sarif-converter)](https://github.com/milkyware/sarif-converter/releases/latest)

- [SARIF Converter](#sarif-converter)
  - [About the Project](#about-the-project)
  - [Installation](#installation)
  - [Upgrading](#upgrading)
  - [Usage](#usage)
    - [Checking version](#checking-version)
    - [Convert SARIF to JUnit](#convert-sarif-to-junit)
  - [Contributing](#contributing)
  - [References](#references)

## About the Project

This is a simple **[Spectre.Console based](https://spectreconsole.net/)** command-line tool for converting SARIF (Static Analysis Results Interchange Format) files into other test result formats such as JUnit and NUnit. It is designed to help integrate static analysis results into CI/CD pipelines and test reporting systems.

## Installation

The tool can be easily installed globally using the `dotnet tool` command:

```sh
dotnet tool install milkyware-sarif-converter -g
```

## Upgrading

The tool can also be easily updated using the `dotnet tool update` command:

```sh
dotnet tool update milkyware-sarif-converter -g
```

## Usage

The tool is bundled with some help documentation which can be access using the `--help` flag:

```sh
milkyware-sarif-converter --help
```

This will return documentation similar to below:

```sh
USAGE:
    milkyware-sarif-converter [OPTIONS]

EXAMPLES:
    milkyware-sarif-converter -i ./.tmp/test.sarif.json -f JUnit
    milkyware-sarif-converter -i ./test.sarif.json -o ./test.xml -f JUnit

OPTIONS:
    -h, --help           Prints help information                                                 
    -v, --version        Prints version information                                              
        --debug          Increase logging verbosity to show all debug logs                       
    -f, --format         Format to convert to                                                    
    -i, --input-file     Path to the input SARIF file                                            
    -o, --output-file    Path to output the converted file to. Outputs to stdout if not specified
```

### Checking version

You can also check what version you currently have installed using the `--version` flag:

```sh
milkyware-sarif-converter --version
```

### Convert SARIF to JUnit

To convert a SARIF file to the ***default*** format, use the following:

```sh
milkyware-sarif-converter --input-file test.sarif
```

The command can also write the resulting converted file direct to disk:

```sh
milkyware-sarif-converter --input-file test.sarif --output-file test.xml
```

The output format can also be manually specified using `--format`:

```sh
milkyware-sarif-converter --input-file test.sarif --output-file test.xml --format JUnit
```

## Contributing

Contributions are welcome! If you have suggestions, bug reports, or want to add new features, please follow these steps:

1. [Fork the repository](https://github.com/milkyware/sarif-converter/fork).
2. Create a new branch for your changes.
3. Make your changes and ensure all tests pass.
4. Submit a pull request with a clear description of your changes.

Please review existing issues and pull requests before starting work. For major changes, open an issue first to discuss your ideas.

## References

- [Spectre.Console](https://spectreconsole.net/)
- [SARIF SDK](https://github.com/Microsoft/sarif-sdk)
- [JUnit Schema](https://github.com/testmoapp/junitxml)
