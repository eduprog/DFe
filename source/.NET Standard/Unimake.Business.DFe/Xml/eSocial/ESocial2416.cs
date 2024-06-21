﻿#pragma warning disable CS1591

using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Unimake.Business.DFe.Servicos;

namespace Unimake.Business.DFe.Xml.ESocial
{
#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.ESocial.ESocial2416")]
    [ComVisible(true)]
#endif
    [Serializable()]
    [XmlRoot("eSocial", Namespace = "http://www.esocial.gov.br/schema/evt/evtCdBenAlt/v_S_01_02_00", IsNullable = false)]
    public class ESocial2416 : XMLBase
    {
        [XmlElement("evtCdBenAlt")]
        public EvtCdBenAlt EvtCdBenAlt { get; set; }
    }

#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.ESocial.EvtCdBenAlt")]
    [ComVisible(true)]
#endif
    public class EvtCdBenAlt
    {
        [XmlAttribute(AttributeName = "Id", DataType = "token")]
        public string ID { get; set; }

        [XmlElement("ideEvento")]
        public IdeEventoESocial2205 IdeEvento { get; set; }

        [XmlElement("ideEmpregador")]
        public IdeEmpregador IdeEmpregador { get; set; }

        [XmlElement("ideBeneficio")]
        public IdeBeneficio IdeBeneficio { get; set; }

        [XmlElement("infoBenAlteracao")]
        public InfoBenAlteracao InfoBenAlteracao { get; set; }
    }

#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.ESocial.IdeBeneficio")]
    [ComVisible(true)]
#endif
    public class IdeBeneficio
    {
        [XmlElement("cpfBenef")]
        public string CpfBenef { get; set; }

        [XmlElement("nrBeneficio")]
        public string NrBeneficio { get; set; }
    }

#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.ESocial.InfoBenAlteracao")]
    [ComVisible(true)]
#endif
    public class InfoBenAlteracao
    {
        [XmlIgnore]
#if INTEROP
        public DateTime DtAltBeneficio { get; set; }
#else
        public DateTimeOffset DtAltBeneficio { get; set; }
#endif

        [XmlElement("dtAltBeneficio")]
        public string DtAltBeneficioField
        {
            get => DtAltBeneficio.ToString("yyyy-MM-dd");
#if INTEROP
            set => DtAltBeneficio = DateTime.Parse(value);
#else
            set => DtAltBeneficio = DateTimeOffset.Parse(value);
#endif
        }

        [XmlElement("dadosBeneficio")]
        public DadosBeneficioESocial2416 DadosBeneficio { get; set; }
    }

#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.ESocial.DadosBeneficioESocial2416")]
    [ComVisible(true)]
#endif
    public class DadosBeneficioESocial2416
    {
        [XmlElement("tpBeneficio")]
        public string TpBeneficio { get; set; }

        [XmlElement("tpPlanRP")]
        public PlanoSegregacaoDaMassa TpPlanRP { get; set; }

        [XmlElement("dsc")]
        public string Dsc { get; set; }

        [XmlElement("indSuspensao")]
        public SimNaoLetra IndSuspensao { get; set; }

        [XmlElement("infoPenMorte")]
        public InfoPenMorteESocial2416 InfoPenMorte { get; set; }

        [XmlElement("suspensao")]
        public Suspensao Suspensao { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeDscField() => !string.IsNullOrEmpty(Dsc);

        #endregion
    }

#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.ESocial.InfoPenMorteESocial2416")]
    [ComVisible(true)]
#endif
    public class InfoPenMorteESocial2416
    {
        [XmlElement("tpPenMorte")]
        public TpPenMorte TpPenMorte { get; set; }
    }

#if INTEROP
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Unimake.Business.DFe.Xml.ESocial.Suspensao")]
    [ComVisible(true)]
#endif
    public class Suspensao
    {
        [XmlElement("mtvSuspensao")]
        public MtvSuspensao MtvSuspensao { get; set; }

        [XmlElement("dscSuspensao")]
        public string DscSuspensao { get; set; }

        #region ShouldSerialize

        public bool ShouldSerializeDscSuspensaoField() => !string.IsNullOrEmpty(DscSuspensao);

        #endregion
    }
}
