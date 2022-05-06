﻿using System;

namespace Unimake.Exceptions
{
    /// <summary>
    /// Classe de exceção quando o certificado digital não é localizado ou está com falha
    /// </summary>
    public class CertificadoDigitalException : Exception
    {
        /// <summary>
        /// Exceção quando o certificado digital não é localizado ou está com falha
        /// </summary>
        public CertificadoDigitalException()
            : base("Certificado digital não localizado ou o mesmo está com falha.") { }

    }

    /// <summary>
    /// Classe de exceção quando ocorre erros na validação dos XML
    /// </summary>
    public class ValidarXMLException : Exception
    {
        /// <summary>
        /// Exceção quando ocorre erros na validação dos XML
        /// </summary>
        /// <param name="message">Mensagem para exceção</param>
        public ValidarXMLException(string message)
            : base(message) { }
    }

#if INTEROP

    /// <summary>
    /// Classe para capturar dados da exception para disponibilizar a outras linguagens de programação, que não .NET, com tipos simples.
    /// </summary>
    public class ThrowHelper
    {
        /// <summary>
        /// Mensagem da exceção gerada
        /// </summary>
        public static string Message { get; set; }

        /// <summary>
        /// Setar a mensagem de erro da exceção para que outras linguagens consigam pegar o erro através desta propriedade
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void Throw(Exception ex) => Message = ex.GetLastException().Message;

        /// <summary>
        /// Recuperar o conteúdo da mensagem de exceção
        /// </summary>
        public string GetMessage() => ThrowHelper.Message;
    }

#endif
}
