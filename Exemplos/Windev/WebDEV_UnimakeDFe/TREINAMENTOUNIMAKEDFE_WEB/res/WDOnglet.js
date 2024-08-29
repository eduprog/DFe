/*! 28.0.2.0 */
/*! VersionVI: 01F280095g   */
var WDOnglet=function(){"use strict";function n(n,t,i,r,u){var f;if(arguments.length){WDChamp.prototype.constructor.apply(this,arguments);var o=u[0],s=u[1],e=u[2];for(this.m_nNbOnglets=o,this.m_tabOnglets=[],this.m_ePosition=s,this.m_tabStyleMasque=e,this.m_tabStyleAffiche=[],f=e.length-1;0<=f;f--)this.m_tabStyleAffiche.push(e[f])}}var t=function(){function n(n,t){var i=n.m_sAliasChamp+"_"+(t+1),r,u;this.m_oTitre=document.getElementById(i);this.m_oCorps=document.getElementById("dww"+i);this.m_oLien=this.m_oTitre.getElementsByTagName("a")[0];this.m_nIndiceVolet=t;this.m_oTitre&&(r=this,u=n,this.m_fOnClick=this.m_oTitre.onclick,this.m_oTitre.onclick=function(n){r.OnClick(n||event,u)})}var t=0,i=1,r=2,u=3;return n.prototype.OnClick=function(n,t){var r,i,u,f;if(this.m_oLien)if(bIE){if(this.m_oLien.disabled)return}else for(r=this.m_oLien.attributes,u=r.length,i=0;i<u;i++)if(f=r.item(i),f.nodeName.toLowerCase()==="disabled")return;t.OnClick(n,this.m_nIndiceVolet);this.m_fOnClick&&this.m_fOnClick.apply(this.m_oTitre,[n])},n.prototype.Affiche=function(n,f){var o,e;if(undefined===this.m_bAffiche||n!=this.m_bAffiche){clWDUtil.SetDisplay(this.m_oCorps,n);n&&clWDUtil.m_oNotificationsImagesVisibles.LanceNotifications(this,this.m_oCorps);o=n?f.m_tabStyleAffiche:f.m_tabStyleMasque;clWDUtil.RemplaceClassName(this.m_oTitre,o[0],o[1]);e=n?"0":"1px";switch(f.m_ePosition){case t:this.m_oTitre.style.borderBottomWidth=e;break;case i:this.m_oTitre.style.borderTopWidth=e;break;case r:this.m_oTitre.style.borderRightWidth=e;break;case u:this.m_oTitre.style.borderLeftWidth=e}bIE||(this.m_oTitre.style.borderCollapse=n?"separate":"collapse");AppelMethode(WDChamp.prototype.ms_sOnDisplay,[this.m_oCorps,n]);window.$&&$(window).trigger("trigger.wb.disposition.visible.maj")}},n.prototype.bActif=function(){return this.m_oLien.disabled?!1:this.m_oLien.readOnly||this.m_oLien.attributes.READONLY?!1:!0},n.prototype.SetHauteur=function(n){this.m_oCorps.firstElementChild.firstElementChild.firstElementChild.height=clWDUtil.GetDimensionPxPourStyle(n)},n}();return n.prototype=new WDChamp,n.prototype.constructor=n,n.prototype.Init=function(){WDChamp.prototype.Init.apply(this,arguments);for(var i=this.m_nNbOnglets,n=0;n<i;n++)this.m_tabOnglets.push(new t(this,n))},n.prototype.OnClick=function(n,t){this.m_tabOnglets[t].bActif()&&this.AfficheVolet(t);this.RecuperePCode(this.ms_nEventNavModifSimple)(n)},n.prototype.GetValeur=function(n,t,i){var r=parseInt(t,10);return WDChamp.prototype.GetValeur.apply(this,[n,r,i])},n.prototype.SetValeur=function(n,t){WDChamp.prototype.SetValeur.apply(this,arguments);var i=parseInt(t,10);return!isNaN(i)&&i>=1&&this.AfficheVolet(i-1),i},n.prototype.GetProp=function(n){switch(n){case this.XML_CHAMP_PROP_NUM_OCCURRENCE:return this.m_nNbOnglets;default:return WDChamp.prototype.GetProp.apply(this,arguments)}},n.prototype.AfficheVolet=function(n){for(var i=this.m_nNbOnglets,t=0;t<i;t++)t!=n&&this.m_tabOnglets[t].Affiche(!1,this);this.m_tabOnglets[n].Affiche(!0,this);this._vSetValeurChampFormulaire(n+1)},n.prototype.HauteurVolet=function(n){for(var i=this.m_nNbOnglets,t=0;t<i;t++)this.m_tabOnglets[t].SetHauteur(n)},n.prototype.OnDisplay=function(n,t){(WDChamp.prototype.OnDisplay.apply(this,arguments),t)&&window.$&&$(window).trigger("wbOngletVisible")},n}(),WDBandeauDefilant=function(){"use strict";function n(){arguments.length&&WDChampParametres.prototype.constructor.apply(this,arguments)}return n.prototype=new WDChampParametres,n.prototype.constructor=n,n.prototype._vLiaisonHTML=function(){WDChampParametres.prototype._vLiaisonHTML.apply(this,arguments);this.m_oChampJQuery=$(_JGE(this.m_sAliasChamp,document))},n.prototype._vAppliqueParametres=function(){WDChampParametres.prototype._vAppliqueParametres.apply(this,arguments);var n=this.m_oChampJQuery;n.wbDefilementSet(this.m_oParametres.m_bDefilementActive);clWDUtil.bForEach(this.m_oDonnees.m_tabPlans,function(t,i){return n.wbPlanVisibleSet(i+1,t.m_bVisible),!0})},n.prototype.SetValeur=function(n,t){return t=WDChampParametres.prototype.SetValeur.apply(this,arguments),this.m_oChampJQuery.wbPlanSet(t)},n.prototype.GetValeur=function(n,t,i){return t=WDChampParametres.prototype.GetValeur.apply(this,arguments),parseInt(i.value,10)},n.prototype.__SetDefilement=function(n){this.m_oParametres.m_bDefilementActive=n;this.m_oChampJQuery.wbDefilementSet(n)},n.prototype.Lance=function(){this.__SetDefilement(!0)},n.prototype.Arrete=function(){this.__SetDefilement(!1)},n.prototype.Premier=function(){this.m_oChampJQuery.wbPlanSet(1,{bBoucle:!0,bSensCorrectionIncrement:!0})},n.prototype.Precedent=function(){this.m_oChampJQuery.wbPlanAvanceRecule(!1,!0)},n.prototype.Suivant=function(){this.m_oChampJQuery.wbPlanAvanceRecule(!0,!0)},n.prototype.Dernier=function(){this.m_oChampJQuery.wbPlanSet(this.m_oChampJQuery.wbPlanOccurrenceGet(),{bBoucle:!0,bSensCorrectionIncrement:!1})},n}(),WDKanban=function(){"use strict";function n(){arguments.length&&(WDChampParametres.prototype.constructor.apply(this,arguments),this.m_KanbanAPI={apiUI:undefined,root:undefined,onInit:undefined},this.m_nTimerAppelMiseAJourUI=0)}var r=104,u=105,t=106,f=107,e="KANBANDEBDEP",o="KANBANPENDEP",i="KANBANFINDEP",s="KANBANDETAIL";return n.prototype=new WDChampParametres,n.prototype.constructor=n,n.prototype.SetProp=function(n,t,i){switch(n){case this.XML_CHAMP_PROP_NUM_ETAT:return this.m_oParametres.etat=i,this.__NotifieParametresDonnees(),WDChampParametres.prototype.SetProp.apply(this,arguments);default:return WDChampParametres.prototype.SetProp.apply(this,arguments)}},n.prototype.__NotifieParametresDonnees=function(n,t){this.m_oParametres=n||this.m_oParametres;this.m_oDonnees=t||this.m_oDonnees;this.m_KanbanAPI.apiUI&&this.m_KanbanAPI.apiUI.SetParametresDonnees(n||this.m_oParametres,t||this.m_oDonnees)},n.prototype._vLiaisonHTML=function(){WDChampParametres.prototype._vLiaisonHTML.apply(this,arguments),function(){if(this.m_KanbanAPI.root)this.__NotifieParametresDonnees();else{this.m_oInput=document.getElementsByName(this.m_sAliasChamp+"_DATA")[0];var n=this.m_oInput.parentElement;(function h(){function c(n){return n===undefined?!0:NSPCS.NSUtil.bGetBooleen(n)}function l(n,t,i){var r=Array.from(n),u=r.splice(t,1)[0];return r.splice(i,0,u),r}if(!window.wbKanbanFactory){requestAnimationFrame(h.bind(this));return}this.m_KanbanAPI.onInit=function(){var t=this.m_oDonnees.list.reduce(function(n,t){return n+t.card.length},0)>50;t||$(n).hide();$(this.m_oInput).parents().removeClass("wbKanbanLoading");t||$(n).fadeIn()}.bind(this);this.m_KanbanAPI.onChange=function(n,t){this.m_oDonnees=n;this.m_oParametres=t;this.__MajInput()}.bind(this);this.m_KanbanAPI.SurEvenementDetails=function(n,t,i){var r=c(clWDUtil.pfGetTraitement(this.m_sAliasChamp,f)(n,s,this.__CreeDinoCarte(t,i)));return this.__NotifieParametresDonnees(),r}.bind(this);this.m_KanbanAPI.SurEvenementDragDebut=function(n,t){var i=this.GetListeEtCarteIndiceDepuisCarteId(t);if(!i)return!0;var u=i.list,f=i.card,o=c(clWDUtil.pfGetTraitement(this.m_sAliasChamp,r)(n,e,this.__CreeDinoCarte(u,f)));return this.__NotifieParametresDonnees(),o}.bind(this);this.m_KanbanAPI.SurEvenementDragEnCours=function(n,t,i,r,f){var e=this.nGetListeIndice(t),s=i,h=NSPCS.NSChamps.oGetObjet(r,142,undefined),l=f,a=c(clWDUtil.pfGetTraitement(this.m_sAliasChamp,u)(n,o,this.__CreeDinoCarte(e,s),h,l+1));return this.__NotifieParametresDonnees(),a}.bind(this);this.m_KanbanAPI.SurEvenementDragFin=function(n,r,u,f,e,o){var s=this.nGetListeIndice(e),h=this.nGetListeIndice(u),a=o,nt=NSPCS.NSChamps.oGetObjet(u,142,undefined),v=f,w=s!==h||a!==v,b,k,p,y,d,g;return w&&(b=this.m_oDonnees.list[h].card.splice(v,1)[0],s>-1&&(k=this.m_oDonnees.list[s].card.push(b)-1,this.m_oDonnees.list[s].card=l(this.m_oDonnees.list[s].card,k,a))),p=c(clWDUtil.pfGetTraitement(this.m_sAliasChamp,t)(n,i,this.__CreeDinoCarte(s,a),nt,v+1)),!p&&w&&this.m_oDonnees.list[h]&&(y=this.GetListeEtCarteIndiceDepuisCarteId(r),y&&y.list===s&&y.card===a&&(d=this.m_oDonnees.list[s].card.splice(a,1)[0],g=this.m_oDonnees.list[h].card.push(d)-1,this.m_oDonnees.list[s].card=l(this.m_oDonnees.list[h].card,g,Math.min(v,this.m_oDonnees.list[h].length)))),this.__NotifieParametresDonnees(),p}.bind(this);this.m_KanbanAPI.DragCarte_Debut=function(n){var i=this.GetListeEtCarteIndiceDepuisCarteId(n),t,r,u;return i?(t=i.list,r=i.card,!this.m_oDonnees.list[t].cascade)?!0:(u=NSPCS.NSChamps.oGetChamp(this.m_oDonnees.list[t].cascade.alias,31).iGetProcedure([["DragCarte_Debut","DRAGCARTE_DEBUT"],["DragCard_Start","DRAGCARD_START"]])(this.__CreeDinoCarte(t,r)),this.__NotifieParametresDonnees(),u):!0}.bind(this);this.m_KanbanAPI.DragCarte_Drop=function(n){var e=this.GetListeEtCarteIndiceDepuisCarteId(n),r,u,f,o,s;return e?(r=e.list,u=e.card,this.m_oDonnees.list[r].cascade&&(f=JSON.parse(JSON.stringify(this.m_oDonnees.list[r].card[u])),NSPCS.NSChamps.oGetChamp(this.m_oDonnees.list[r].cascade.alias,31).iGetProcedure([["DragCarte_Drop","DRAGCARTE_DROP"],["DragCard_Drop","DRAGCARD_DROP"]])(this.CreeDinoCarteDepuisCarteId(n,f),u+1),NSPCS.NSChamps.oGetChamp(this.m_oDonnees.list[r].cascade.alias,31).iGetProcedure([["DragCarte_Fin","DRAGCARTE_FIN"],["DragCard_End","DRAGCARD_END"]])(this.CreeDinoCarteDepuisCarteId(n,f))),o=NSPCS.NSChamps.oGetObjet(this.m_oDonnees.list[r].id,142,undefined),s=c(clWDUtil.pfGetTraitement(this.m_sAliasChamp,t)(event,i,this.CreeDinoCarteDepuisCarteId(n,f),o,u+1)),this.__NotifieParametresDonnees(),s):!0}.bind(this);this.m_KanbanAPI.DragCarte_EntreeSurvol=function(n){var i=this.GetListeEtCarteIndiceDepuisCarteId(n),t,r,u;return i?(t=i.list,r=i.card,!this.m_oDonnees.list[t].cascade)?!0:(u=NSPCS.NSChamps.oGetChamp(this.m_oDonnees.list[t].cascade.alias,31).iGetProcedure([["DragCarte_EntreeSurvol","DRAGCARTE_ENTREESURVOL"],["DragCard_EnterHover","DRAGCARD_ENTERHOVER"]])(this.__CreeDinoCarte(t,r,!0)),this.__NotifieParametresDonnees(),u):!0}.bind(this);this.m_KanbanAPI.DragCarte_SortieSurvol=function(n){var i=this.GetListeEtCarteIndiceDepuisCarteId(n),t,r,u;return i?(t=i.list,r=i.card,!this.m_oDonnees.list[t].cascade)?!0:(u=NSPCS.NSChamps.oGetChamp(this.m_oDonnees.list[t].cascade.alias,31).iGetProcedure([["DragCarte_FinSurvol","DRAGCARTE_FINSURVOL"],["DragCard_ExitHover","DRAGCARD_EXITHOVER"]])(this.__CreeDinoCarte(t,r,!0)),this.__NotifieParametresDonnees(),u):!0}.bind(this);this.m_KanbanAPI.DragCarte_Fin=function(n){var i=this.GetListeEtCarteIndiceDepuisCarteId(n),t,r,u;return i?(t=i.list,r=i.card,!this.m_oDonnees.list[t].cascade)?!0:(u=NSPCS.NSChamps.oGetChamp(this.m_oDonnees.list[t].cascade.alias,31).iGetProcedure([["DragCarte_Fin","DRAGCARTE_FIN"],["DragCard_End","DRAGCARD_END"]])(this.__CreeDinoCarte(t,r,!0)),this.__NotifieParametresDonnees(),u):!0}.bind(this);this.m_KanbanAPI.root=window.wbKanbanFactory({domHost:n,domInputHidden:this.m_oInput,params:this.m_oParametres,data:this.m_oDonnees,exportAPI:this.m_KanbanAPI,setExportAPI:function(n){this.m_KanbanAPI.apiUI=n}.bind(this)})}).bind(this)()}}.bind(this)()},n.prototype._voFusionneDonne=function(n){WDChampParametres.prototype._voFusionneDonne.apply(this,arguments);this.__NotifieParametresDonnees(this.m_oParametres,n||this.m_oDonnees)},n.prototype.__MajInput=function(){for(var i={},t=Object.keys(this.m_oDonnees),n=0;n<t.length;++n){if(t[n]==="list"){i.list=this.m_oDonnees.list.map(function(n){for(var r={},i=Object.keys(n),t=0;t<i.length;++t){switch(i[t]){case"cascade":case"title":case"style":continue}if(i[t]==="card"){r.card=n.card.map(function(n){for(var r={},i=Object.keys(n),t=0;t<i.length;++t){switch(i[t]){case"include":case"body":case"js":case"link":case"style":case"onload":case"callonchange":continue}r[i[t]]=n[i[t]]}return r});continue}r[i[t]]=n[i[t]]}return r});continue}i[t[n]]=this.m_oDonnees[t[n]]}this.m_oInput.value=JSON.stringify(i)},n.prototype._vAppliqueParametres=function(n){WDChampParametres.prototype._vAppliqueParametres.apply(this,arguments);this.__NotifieParametresDonnees(n||this.m_oParametres,this.m_oDonnees)},n.prototype.SetValeur=function(){},n.prototype.GetValeur=function(){},n.prototype.GetInfoXY=function(n,t,i){return this.m_KanbanAPI.apiUI.GetInfoXY(t,i,n)},n.prototype.OnModifCarte=function(n){if(n.callonchange){var t=n.id;this.m_oDonnees.change?this.m_oDonnees.change.indexOf(t)===-1&&this.m_oDonnees.change.push(t):this.m_oDonnees.change=[t];this.__MajInput();this.m_nTimerAppelMiseAJourUI&&window.cancelIdleCallback(this.m_nTimerAppelMiseAJourUI);this.m_nTimerAppelMiseAJourUI=window.requestIdleCallback(function(){this.m_oDonnees.change&&this.m_oDonnees.change.length&&(_JAEE(_PAGE_,this.m_sAliasChamp,"KANBANMODIF",9),this.m_oDonnees.change=undefined)}.bind(this))}},n.prototype.RazMajEnAttente=function(){this.m_oDonnees.change=undefined},n.prototype.nGetNbListes=function(){return this.m_oDonnees.list.length},n.prototype.GetListe=function(n){return this.m_oDonnees.list.find(function(t){return t.id===n})},n.prototype.GetListeParIndice=function(n){return this.m_oDonnees.list[n]},n.prototype.GetListeDepuisNom=function(n){return this.m_oDonnees.list.find(function(t){return t.name===n})},n.prototype.GetListeEtCarteIndiceDepuisCarteId=function(n){for(var i,t=0;t<this.m_oDonnees.list.length;++t)for(i=0;i<this.m_oDonnees.list[t].card.length;++i)if(n==this.m_oDonnees.list[t].card[i].id)return{list:t,card:i};return},n.prototype.CreeDinoCarteDepuisCarteId=function(n,t){var i=this.GetListeEtCarteIndiceDepuisCarteId(n);return i?this.__CreeDinoCarte(i.list,i.card,!0):NSPCS.NSTypes.oCreeKBCarteDepuisChamp(t,undefined)},n.prototype.GetCarte=function(n,t){var i=this.GetListe(n);return i?i.card[t]:undefined},n.prototype.nGetNbCartes=function(n){var t=this.GetListe(n);return t?t.card.length:0},n.prototype.nGetNbCartesMax=function(n){var t=this.GetListe(n);return t&&undefined!==t.max?t.max:-1},n.prototype.nGetCarteIndice=function(n,t){var i=this.GetListe(n);return i.card.findIndex(function(n){return n.id===t.id})},n.prototype.nGetListeIndice=function(n){return this.m_oDonnees.list.findIndex(function(t){return t.id===n})},n.prototype.__CreeDinoCarte=function(n,t,i){var r=this.m_oDonnees.list[n],u=r.card[t];return i||(this.m_oDonnees.selected=u.id,this.__MajInput()),NSPCS.NSTypes.oCreeKBCarteDepuisModele(u,this.m_sAliasChamp,r.id,n)},n}(),WDTCD=function(){"use strict";function n(n,t,i,r,u){arguments.length&&(WDChampParametres.prototype.constructor.apply(this,arguments),this.m_TCDAPI={apiUI:undefined,root:undefined,onInit:undefined},this.SetParametres.apply(this,u))}return n.prototype=new WDChampParametres,n.prototype.constructor=n,n.prototype.SetProp=function(n,t,i){switch(n){case this.XML_CHAMP_PROP_NUM_ETAT:return this.m_oParametres.etat=i,this.__NotifieParametresDonnees(),WDChampParametres.prototype.SetProp.apply(this,arguments);default:return WDChampParametres.prototype.SetProp.apply(this,arguments)}},n.prototype.__NotifieParametresDonnees=function(n,t,i){this.m_oParametres=n||this.m_oParametres;this.m_oDonnees=t||this.m_oDonnees;this.m_oContenu=i||this.m_oContenu;this.__MajInput();this.m_TCDAPI.apiUI&&this.m_TCDAPI.apiUI.SetParametresDonnees(this.m_oParametres,this.m_oDonnees,this.m_oContenu)},n.prototype._vLiaisonHTML=function(){WDChampParametres.prototype._vLiaisonHTML.apply(this,arguments),function(){if(this.m_TCDAPI.root)this.__NotifieParametresDonnees();else{this.m_oInput=document.getElementsByName(this.m_sAliasChamp+"_DATA")[0];var n=this.m_oInput.parentElement;(function t(){if(!window.wbTCDFactory){requestAnimationFrame(t.bind(this));return}var i=$(this.m_oInput).parents(".wbTCDLoading");this.m_TCDAPI.onInit=function(){$(n).hide();i.removeClass("wbTCDLoading");$(n).fadeIn()}.bind(this);this.m_TCDAPI.onChange=function(n,t){this.m_oDonnees=n;this.m_oParametres=t;this.__MajInput()}.bind(this);this.m_TCDAPI.SurFAA=function(n,t){if(n==="drillUp"){var i=t.nLigne,r=t.nProfondeur;setTimeout(function(){this.__NotifieParametresDonnees()}.bind(this),3e3)}}.bind(this);this.m_TCDAPI.SurEvenementSelectionCellule=function(){return this.__NotifieParametresDonnees(),!1}.bind(this);this.m_TCDAPI.root=window.wbTCDFactory({domHost:n,domInputHidden:this.m_oInput,params:this.m_oParametres,value:this.m_oDonnees,data:this.m_oContenu,exportAPI:this.m_TCDAPI,setExportAPI:function(n){this.m_TCDAPI.apiUI=n}.bind(this)})}).bind(this)()}}.bind(this)()},n.prototype._voFusionneDonne=function(n){WDChampParametres.prototype._voFusionneDonne.apply(this,arguments);this.__NotifieParametresDonnees(this.m_oParametres,n||this.m_oDonnees,this.m_oContenu)},n.prototype.SetParametres=function(n,t,i){this.m_oContenu=i;WDChampParametres.prototype.SetParametres.apply(this,arguments)},n.prototype.__MajInput=function(){if(this.m_oInput){var n=this.m_oDonnees;this.m_oInput.value=JSON.stringify(n)}},n.prototype._vAppliqueParametres=function(n){WDChampParametres.prototype._vAppliqueParametres.apply(this,arguments);this.__NotifieParametresDonnees(n||this.m_oParametres,this.m_oDonnees,this.m_oContenu)},n.prototype.SetValeur=function(){},n.prototype.GetValeur=function(){},n.prototype.OnModifCellule=function(){},n.prototype.oGetPosition=function(){return},n.prototype.__CreeDinoTCDPosition=function(){return},n}()