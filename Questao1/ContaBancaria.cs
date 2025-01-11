using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        public int numero { get; set; }

        public string titular { get; set; }

        public double saldo { get; set; }



        public ContaBancaria(int numero, string titular)
        {
            this.numero = numero;
            this.titular = titular;
        }

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            this.numero = numero;
            this.titular = titular;
            this.saldo = depositoInicial;
        }

        public void Deposito(double quantia)
        {
            saldo += quantia;   // method scope, no error  
        }

        public void  Saque(double quantia)
        {
            saldo -= quantia;   // method scope, no error  
            saldo -= 3.50;
        }
    }
}
