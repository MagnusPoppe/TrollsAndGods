# Trolls And Gods

#### Laget av:
- Magnus Poppe Wang
- Ruben Bratsberg
- Håvard Torp Helmersen
- Håkon Mølstre

## OM PROSJEKTET
Dette er en bacheloroppgave for Høgskolen i Sørøst Norge for Informatikk linjen. Under har du den offisielle prosjektbeskrivelsen levert inn som "Milepæl 1". 

## BAKGRUNN
Skrevet den 27.januar 2017
Håkon Mølstre Ruben Bratsberg
Etter å ha lært svært mye programmering i skolesammenheng, så vi etter en utfordring til å lage noe kreativt. Vi har alle mange ideer og visjoner, og stor interesse av spill. Bachelorprosjektet var dermed et naturlig punkt for oss å sette av nødvendig tid og ressurser til å jobbe med et stort prosjekt av eget initiativ, som vi alle brenner for. Det ligger mye inspirasjon fra eldre spill i grunn for valg av tema, men vi mener det er rom for nyskapning og ubrukt potensiale i sjangeren vi har valgt.

## BEHOV OG VERKTØY
Til prosjektet er det behov for verktøy til utvikling og en plass på skolelokale til å drive denne utviklingen. Gjennom skolen fikk gruppa egen kontorplass slik at vi kan la datamaskiner og Kanban-tavle stå mellom arbeidsdagene våre.
Til utvikling er det flere verktøy som er nødvendig for å skrive kode og sette det sammen, samt også ha en spillmotor å bygge opp spillet vårt i. Til kode bruker vi  MonoDevelop  og  Visual Studio  for å skrive i C#, som har egne utvidelser for å være integrert med spillmotoren vi bruker,  Unity3D . Behovene for datamaskiner og tilbehør dekker vi selv med utstyr hjemmefra, eller med tilbehør vi låner fra skolen.

## RAMME
Vi innser at vi har satt oss ut for et prosjekt som er for stort for en bacheloroppgave. Vi har derfor bestemt oss for å forsøke å nå en spillbar prototype heller enn et ferdig spill. Funksjonalitet vi vet at vi ikke kan fullføre:
- Vi skal ikke ha alle grafiske elementer på plass, men heller en grov skisse over hvordan det ferdige produktet ville sett ut på slutten av en utviklingsperiode.
- Animasjoner vil være mangelfullt, i beste fall enkelteksempler for å vise bruken.
- Ingen kunstig intelligens man skal spille mot, prototypen skal kun være fokusert på “spiller-mot-spiller”-modus.
- Ingen perfekt inndeling av enhetenes styrke. Spille vil ved dette aspektet ikke være “balansert”.
- Vi vil kun ha enkelte av byene og enhetene på plass ved prosjektslutt. Vi har ikke kapasitet til dette. Dette er heller ikke nødvendig for å ha en spillbar prototype.
- Vi skal ikke sette opp noen ferdige programmerte spilltjenere for dette prosjektet.

## MÅL
### EFFEKTMÅL
Prosjektet ble hovedsakelig startet opp for å få en ordentlig mulighet for å prøve seg på spilldesign. For alle på gruppen har dette vært noe vi har hatt lyst til å lære oss lenge! Det vil derfor være et mål i seg selv å lære seg Unity spillmotoren. Av prosessen får vi god trening i grafikkprogrammering, ettersom vi skal koble visuelle elementer sammen med rendering og animering.

Programmere i et større prosjekt med flere deltakere er noe ingen av oss har mye erfaring med. For dette prosjektet har vi derfor bestemt at det skal jobbes testdrevet. Å lære å skrive gode enhetstester for å kunne automatisere testprosessen er et viktig læremål for hele prosjektgruppen. Alt dette synkroniseres med bruken av versjonskontroll og GIT.
For dette prosjektet jobber vi i scrum, kombinert med en kanban tavle. Dette anser vi som å være en god struktur for å opprettholde fremgangen i prosjektet. Vi lærer også hvordan man bruker en slik smidig utviklingsmetodikk i praksis.
Med en virkende spillbar prototype skal man kunne ta inn testere for å videre bestemme viktige funksjoner.

### RESULTATMÅL
Målet vårt for dette bachelorprosjektet er å lage en spillbar prototype. Spillbar betyr at den skal ha:
- En måte å bevege spilleren sine tropper
- En måte å spille mot hverandre på, 2 eller flere
- Muligheten til å kunne kjøpe og oppgradere bygninger/tropper i byene spilleren eier
- Et kampsystem
- En måte å vinne på
- Automatisk genererte kart, med bygninger, ressurser og nøytrale monstre
- Turbasert flyt i spillet, rundt kartet og kampsystemet

## FREMDRIFTSPLAN
Vi har laget et Gantt-diagram som representerer fremdriften i prosjektet. Dette er et ganske standard diagram som beskriver Siden vi jobber med metodikken scrum har vi også lagt opp prosjektarbeidet med en produkt backlog. Her har vi lagt opp alle oppgavene vi kan tenke oss. Vi har videre delt disse oppgavene inn i forskjellige “releases” eller lanseringer av spillet. Vi kan ikke jobbe så smidig at vi har test teams på de første “releasene” grunnet oppsettet av spillet. De forskjellige releasene vi har planlagt er:
- Overworld : et overordnet kart der alle byer er plassert. Her skal man ha muligheten til å bevege seg rundt, utforske, og støte på objekter som utløser hendelser. Vi har allerede implementert A* algoritmen for å bestemme raskeste vei på kartet. Vi ønsker også å ha en god løsning på tilfeldig generering av kart når man starter et spill, med variabler spillerne kan justere på før spillet startes.
- Towns : Her skal vi får på plass en fungerende “town” der man kan kjøpe bygninger, enheter/krigere m.m. Når en spiller i overworld klikker på en tilhørende by, skal et vindu dukke opp med visuell fremvisning og grafisk interaksjon.
- Combat:  Når man i overworld støter på en fiende, skal et kampvindu dukke opp. Kampsystemet skal inneholde muligheten for 2 spillere til å krige mot hverandre, eller 1 spiler mot nøytrale tropper. En etter en kriger får utføre en handling, i rekkefølge. En kamp foregår helt til en spiller har flyktet eller mistet alle sine krigere.

## BUDSJETT
Prosjektet har ikke behov for noe større budjsett. Alle gruppemedlemmer har sin egen datamaskin å jobbe på, og maskintilbehør samt også kontorplass har vi fått låne av skolen. Unity, spillutviklingsprogramvaren vår, er også gratis til prosjekt av vår størrelse mens vi er studenter og ikke har noe inntekt på arbeidet over en viss grense.Utgiftene til prosjektet vil være på vegne av kunstansvarlig på gruppa, Benjamin Brinckmann. Benjamin er en ekstern kunster og venn av gruppa som vil være med på prosjektet av egen innsats. Siden han bor i Sandefjord trenger han en scanner og utstyr for å kunne sende den håndtegnede kunsten til oss. 

I vårt prosjekt er det tidsbudsjettet som er mer relevant. Vi har satt opp arbeids fra kl 09:00 til 17:00 hver tirsdag, onsdag og fredag, som gir hver person 24 arbeidstimer i uka. Over de 18 ukene prosjektet foregår har vi altså 1632 timer til disposisjon for prosjektet. Nederst har vi vedlagt et Gantt-diagram som viser hvordan vi fordeler tiden over de forskjellige arbeidsoppgavene våre.

## ORGANISERING
Vi er 4 studenter som arbeider i team faste tidspunkter i uken i et lokale på skolen. Vi har satt av hver tirsdag, onsdag og fredag fra 09:15 til 17:00 til prosjektarbeidet. Disse dagene møtes vi i prosjektlabben for organisert jobbing. Vi starten dagene med en daglig scrum. Her går vi igjennom hva vi jobber med, evt hvilke nye oppgaver vi skal ta på oss. Vi ser på om det er noen problemer og går igjennom om nødvendig.
Vi har lagt opp arbeidet for å benytte oss av scrum arbeidet. Vi begynte hele prosjektet med å legge opp en produkt backlog. Vi har så organisert oppgavene videre i release backlogs. Les mer om disse under punktet “fremdriftsplan”. Når vi er ferdig med en sprint skal vi også ha “retrospektive” møter, der vi går igjennom hva som ble gjort under forrige sprint, og hvordan vi
kan bedre oss frem til neste sprint. Her er det viktig for oss å lære. Slik skal vi jobbe til prosjektet er over.

Med oss i prosjektet har vi også en illustratør, som tar for seg det visuelle vi skal programmere inn i spillet. Han bor ikke i Bø, og arbeider ikke med prosjektet på samme tidspunkter som oss, men kommuniserer og holder oss oppdatert over kommunikasjonsplattformen Slack. Han har allerede kommet på besøk for å planlegge og diskutere visjonen videre, og skal holdes videre oppdatert rundt fremgangen i prosjektet.

#### Rollefordeling og ansvarsforhold:
- Prosjektleder : Magnus
- Scrum master: Magnus
- PR ansvarlig: Ruben
- Nettsideansvarlig: Ruben
- Musikkansvarlig: Håvard
- Front-end: Håvard
- Back-end: Håkon
- Kunstansvarlig: Benjamin Brinckmann (ekstern)

## INFORMASJONSPLAN
Det er ikke mange konkrete interessenter for dette prosjektet. Vi har allerede gjort litt brukerundersøkelse å hva spillerene liker og ikke liker med spillet vi bruker som inspirasjon. Vi planlegger å holde “subredditen” /r/homm oppdatert med fremgangen vår. Dette er et potensielt “test-miljø” også, der vi kan hente ut testere.
Vi har også laget instagram og Twitter konto for å dokumentere prosjektet. Dette, i tillegg til prosjektweb skal gi mye dokumentasjon på prosjektet.

Se posten på reddit:
https://www.reddit.com/r/HoMM/comments/5nr929/we_are_a_team_of_students_making_a_h eroes_3/


## KVALITETSSIKRINGSPLAN
Underveis blir mange metoder og klasser laget, disse må da grundig testes ved hjelp av Visual studio sine test tools og NUnit, for at vi skal være helt sikre på at metodene og klassene fungerer som de skal. Vi skal også ha andre folk til å teste prototypen for å få meninger på hva som fungerer, og hva som ikke fungerer.

## KRITISKE FAKTORER
Tid:  Vi har en klar tidsramme vi må holde oss innenfor og det er derfor viktig å holde god kontroll på hvor mye tid vi bruker på hver release.

Data-ressurser:  prosjektet forutsetter at alle har tilgang til Unity og Github, disse er lett tilgjengelige og representerer liten grad for usikkerhet.

Kontorlokale:  Med egne kontorlokaler er vi mye mer effektive. Mister vi disse vil arbeidet gå betydelig saktere. Faste tidsrammer, tilgjengelig utstyr og arbeidsrom gjør prosessen mer forutsigbar.

Kunnskap:  Vi har liten tidligere erfaring med Unity, C# og spillutvikling, derfor er det viktig at vi lærer raskt og effektivt hvordan vi skal gjøre ting. Slik kan vi på en mer oversiktlig måte planlegge og sette opp en plan for hvor lang tid arbeidsoppgavene vil ta.

Personale:  Stort krav til egeninnsats, vi har ingen oppdragsgiver og er derfor selv helt ansvarlige for at prosjektet blir gjennomført. Kommunikasjon og samarbeid med ekstern illustratør blir viktig for å få tilgang på det grafiske vi ønsker å få spesialtilpasset til vårt spill.

## Vedlegg:
Fremdriftsplan et et gantt diagram bestående av tidslinjen for prosjektet. Du finner den her:
http://byteme.no/document/bachelor_fremdriftsplan.pdf

Prosjektweb er nettsiden der vi legger ut alt av dagbokinnlegg. Du finner den her:
http://byteme.no

Vi har også sosiale medie-kontoer der vi legger ut mye bilder og tekst fra/om utviklingen: Instagram:  www.instagram.com/bytemeusn
Twitter: www.twitter.com/bytemeusn
