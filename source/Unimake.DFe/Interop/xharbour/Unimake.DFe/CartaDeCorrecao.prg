* ---------------------------------------------------------------------------------
* Evento de carta de corre��o eletr�nica da NFe
* ---------------------------------------------------------------------------------
Function CartaDeCorrecao()
 Local oConfiguracao
 Local oEnvEvento, oEvento, oDetEventoCCE, oInfEvento

* Criar configura�ao b�sica para consumir o servi�o
 oConfiguracao = CreateObject("Unimake.Business.DFe.Servicos.Configuracao")
 oConfiguracao:TipoDfe = 0 // 0=nfe
 oConfiguracao:Servico = 5 // 5=Envio de evento
 oConfiguracao:CertificadoSenha = "12345678"
 oConfiguracao:CertificadoArquivo = "C:\Projetos\certificados\UnimakePV.pfx"

 * Criar tag EnvEvento
 oEnvEvento = CreateObject("Unimake.Business.DFe.Xml.NFe.EnvEvento")
 oEnvEvento:Versao = "1.00"
 oEnvEvento:IdLote = "000000000000001"

 * =================================================
 * EVENTO N�MERO 1
 * =================================================
 * Criar tag Evento
 oEvento = CreateObject("Unimake.Business.DFe.Xml.NFe.Evento")
 oEvento:Versao = "1.00"
 
 * Criar tag DetEventoCCE
 oDetEventoCCE = CreateObject("Unimake.Business.DFe.Xml.NFe.DetEventoCCE")
 oDetEventoCCE:Versao = "1.00"
 oDetEventoCCE:XCorrecao = "CFOP errada, segue CFOP correta. teste."

 * Criar tag InfEvento
 oInfEvento = CreateObject("Unimake.Business.DFe.Xml.NFe.InfEvento")
 
 * Adicionar a tag DetEventoCCE dentro da Tag DetEvento
 oInfEvento:DetEvento = oDetEventoCCE
 
 * Atualizar propriedades da oInfEvento
 * IMPORTANTE: Atualiza��o da propriedade TpEvento deve acontecer depois que o DetEvento recebeu o ODetEventoCCE para que funcione sem erro
 oInfEvento:COrgao = 41 // UFBrasil.PR
 oInfEvento:ChNFe = "41191006117473000150550010000579281779843610"
 oInfEvento:CNPJ = "06117473000150"
 oInfEvento:DhEvento = DateTime()
 oInfEvento:TpEvento = 110110 // TipoEventoNFe.CartaCorrecao
 oInfEvento:NSeqEvento = 1
 oInfEvento:VerEvento = "1.00"
 oInfEvento:TpAmb = 2 // TipoAmbiente.Homologacao

 * Adicionar a tag InfEvento dentro da tag Evento
 oEvento:InfEvento = oInfEvento

 * Adicionar a tag Evento dentro da tag EnvEvento
 oEnvEvento:AddEvento(oEvento)

 * =================================================
 * EVENTO N�MERO 2
 * =================================================
 * Criar tag Evento
 oEvento = CreateObject("Unimake.Business.DFe.Xml.NFe.Evento")
 oEvento:Versao = "1.00"
 
 * Criar tag DetEventoCCE
 oDetEventoCCE = CreateObject("Unimake.Business.DFe.Xml.NFe.DetEventoCCE")
 oDetEventoCCE:Versao    = "1.00"
 oDetEventoCCE:XCorrecao = "Transportador errado. teste."

 * Criar tag InfEvento
 oInfEvento = CreateObject("Unimake.Business.DFe.Xml.NFe.InfEvento")
 
 * Adicionar a tag DetEventoCCE dentro da Tag DetEvento
 oInfEvento:DetEvento = oDetEventoCCE

 * Atualizar propriedades da oInfEvento
 * IMPORTANTE: Atualiza��o da propriedade TpEvento deve acontecer depois que o DetEvento recebeu o ODetEventoCCE para que funcione sem erro
 oInfEvento:COrgao     = 41 // UFBrasil.PR
 oInfEvento:ChNFe      = "41191006117473000150550010000579281779843610"
 oInfEvento:CNPJ       = "06117473000150"
 oInfEvento:DhEvento   = DateTime()
 oInfEvento:TpEvento   = 110110 // TipoEventoNFe.CartaCorrecao
 oInfEvento:NSeqEvento = 2
 oInfEvento:VerEvento  = "1.00"
 oInfEvento:TpAmb      = 2 // TipoAmbiente.Homologacao

 * Adicionar a tag InfEvento dentro da tag Evento
 oEvento:InfEvento = oInfEvento

 * Adicionar a tag Evento dentro da tag EnvEvento
 oEnvEvento:AddEvento(oEvento)

 ? oEnvEvento:Versao, oEnvEvento:IdLote
 ? "Qde eventos:", oEnvEvento:GetEventoCount()
  
 For I = 1 To oEnvEvento:GetEventoCount()
     oTagEvento := oEnvEvento:GetEvento(I - 1) 
     ? I, oTagEvento:InfEvento:NSeqEvento, oTagEvento:InfEvento:COrgao
 Next I     

 * Enviar carta de correcao
 oRecepcaoEvento = CreateObject("Unimake.Business.DFe.Servicos.NFe.RecepcaoEvento")
 oRecepcaoEvento:Executar(oEnvEvento,  oConfiguracao)

 ? "CStat do Lote Retornado:", oRecepcaoEvento:Result:CStat, "- XMotivo:", oRecepcaoEvento:Result:XMotivo
 
 if oRecepcaoEvento:Result:CStat == 128 //128 = Lote de evento processado com sucesso.
  * Como pode existir v�rios eventos de corre��o, 
  * � necess�rio fazer um loop para ver a autoriza��o de cada um deles
    For I = 1 To oRecepcaoEvento:Result:GetRetEventoCount()
        oRetEvento = oRecepcaoEvento:Result:GetRetEvento(I - 1)
        SWITCH oRetEvento:InfEvento:CStat
          CASE 135 //Evento homologado com vincula��o da respectiva NFe
          CASE 136 //Evento homologado sem vincula��o com a respectiva NFe (SEFAZ n�o encontrou a NFe na base dela)
          CASE 155 //Evento de Cancelamento homologado fora do prazo permitido para cancelamento 
               oRecepcaoEvento:GravarXmlDistribuicao("tmp\testenfe") //Grava o XML de distribui��o
               Exit
		   
       #Ifdef __XHARBOUR__
          DEFAULT
       #Else
          OTHERWISE    
       #endif
                    // Evento rejeitado
               // Realizar as a��es necess�rias
               Exit
        END
		
        ? "CStat do evento", AllTrim(Str(I,10)), "retornado:", oRetEvento:InfEvento:CStat, "- xMotivo:", oRetEvento:InfEvento:XMotivo
    Next
 EndIf 

 Wait
RETURN