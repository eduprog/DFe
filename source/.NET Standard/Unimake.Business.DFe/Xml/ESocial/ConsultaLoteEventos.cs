﻿#pragma warning disable CS1591
using System;
using System.Xml;
using System.Xml.Serialization;
#if INTEROP
using System.Runtime.InteropServices;
#endif

namespace Unimake.Business.DFe.Xml.ESocial
{
    #region Consultar Lote Eventos
#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.ESocial.ConsultarLoteEventos")]
    [ComVisible(true)]
#endif
    [Serializable()]
    [XmlRoot("eSocial", Namespace = "http://www.esocial.gov.br/schema/lote/eventos/envio/consulta/retornoProcessamento/v1_0_0", IsNullable = false)]
    public class ConsultarLoteEventos : XMLBase
    {
        [XmlElement("consultaLoteEventos")]
        public ConsultaLoteEventos ConsultaLoteEventos { get; set; }
    }
    #endregion Consulta Lote Eventos

#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.ESocial.ConsultaLoteEventos")]
    [ComVisible(true)]
#endif
    public class ConsultaLoteEventos
    {
        [XmlElement("protocoloEnvio")]
        public string ProtocoloEnvio { get; set; }
    }
}