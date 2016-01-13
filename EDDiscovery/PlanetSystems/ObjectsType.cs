using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{
    public class ObjectsType
    {



        public void ParseJson()
        {
            string json = @"{
DescriptionItem: [
                {
            Name: 'ClassO',
		Short: 'Class O Star',
		Description: 'Class O type stars are the most luminous and massive main sequence stars in the galaxy. They range in mass from 15 to 90 solar masses and burn very brightly indeed, with a surface temperature reaching 52,000 K so appear very blue. They are very short lived with lifetimes of 1 - 10 million years, ending in supernova.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassB',
		Short: 'Class B Star',
		Description: 'Class B stars are very luminous blue-white stars. They range in mass from 2 to 16 solar masses and have a surface temperature reaching 30,000 K. Their lifetimes are shorter than most main sequence stars.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassA',
		Short: 'Class A Star',
		Description: 'Class A stars are hot white or bluish white main sequence stars. They range in mass from 1.4 to 2.1 solar masses and have a surface temperature reaching 10,000 K.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassF',
		Short: 'Class F Star',
		Description: 'Class F stars are white main sequence stars. They range in mass from 1 to 1.4 solar masses and have a surface temperature reaching 7,600 K.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassG',
		Short: 'Class G Star',
		Description: 'Class G stars are white-yellow main sequence stars. They range in mass from 0.8 to 1.2 solar masses and have a surface temperature reaching 6,000 K.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassK',
		Short: 'Class K Star',
		Description: 'Class K stars are yellow-orange main sequence stars with a long and generally stable life. They range in mass from 0.6 to 0.9 solar masses and have a surface temperature reaching 5,000 K.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassM',
		Short: 'Class M Star',
		Description: 'Class M stars are red stars that form the bulk of the main sequence stars in the galaxy. Their mass is low, as is their surface temperature.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassL',
		Short: 'Class L Dwarf',
		Description: 'Class L dwarfs are dwarf stars that are cooler than M class stars. They are on the borderline of supporting fusion of hydrogen in their cores, and their temperatures range from 1,300 K to 2,400 K, cool enough to have alkaline metals and metal hydrides in their atmospheres.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassT',
		Short: 'Class T Dwarf',
		Description: 'Class T dwarfs are brown dwarfs with a surface temperature between 700 and 1,300 K. They are sometimes known as Methane Dwarfs due to the prominence of methane in their composition. They are on the borderline between what might be considered a very large gas giant planet and a star.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassY',
		Short: 'Class Y Dwarf',
		Description: 'Class Y dwarfs are the coolest of the brown dwarfs. Surface temperatures are less than 700 K, and are effectively very large gas giant planets, with some stellar properties.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassTT',
		Short: 'T Tauri Star',
		Description: 'T Tauri type stars are very young stars which are in the process of gravitational contraction.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassGA',
		Short: 'Class A Supergiant Star',
		Description: 'Class A blue-white supergiant star. It is approaching the end of its life and hydrogen burning has stopped in its core, and star has begun expanding towards being red supergiant.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassGF',
		Short: 'Class F Supergiant Star',
		Description: 'Class F yellow-white supergiant star. It is approaching the end of its life and hydrogen burning has stopped in its core, and star has begun expanding towards being red supergiant.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassGK',
		Short: 'Class K Giant Star',
		Description: 'Orange giant stars with spectral type K. Orange giants like this are reaching the end of their lives, and have moved off the main sequence. Hydrogen has stopped fusing in the core, leaving a collapsed core of degenerate helium, and hydrogen burning is taking place in an outer shell as the star continues to expand.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassGM',
		Short: 'Class M Giant Star',
		Description: 'Red giant star. Red giants are in the latter part of their lives. Hydrogen has stopped fusing in their degenerate helium cores and has moved to an outer shell, causing the star to expand. The outer atmosphere is inflated and tenuous, and the surface temperature is below 5,000 K.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassSGM',
		Short: 'Class M Supergiant Star',
		Description: 'Red supergiants are massive stars near the end of their lives. They have entered a helium burning phase, where helium is fused into carbon and oxygen. They have enormous sizes swelling up to many hundred solar diameters - up to 7 AU in some cases. their out reaches can be quite cool - typically 3500-4500 K.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassH',
		Short: 'Herbig Ae/Be Star',
		Description: 'Herbig Ae/Be stars are young stars typically less than 10 million years old with characteristics of either A or B class main sequence stars. They are usually between 2 and 8 solar masses. The mass of the proto-star determines its spectral class when it joins the main sequence.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassW',
		Short: 'Wolf-Rayet Star',
		Description: 'Wolf-Rayet class stars are massive stars that are nearing the end of their life cycle and have moved out of their hydrogen-burning phase. They were once over 20 solar masses but now shed considerable amounts of material through solar wind. Their surface temperature can reach 200,000 K, so they appear a brilliant blue.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassWN',
		Short: 'Wolf-Rayet N Star',
		Description: 'Wolf-Rayet class stars are massive stars that are nearing the end of their life cycle and have moved out of their hydrogen-burning phase. They were once over 20 solar masses but now shed considerable amounts of material through solar wind. Their surface temperature can reach 200,000 K, so they appear a brilliant blue. This star is a variant of the Wolf-Rayet stars whose spectrum is dominated by ionised nitrogen and helium lines.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassWNC',
		Short: 'Wolf-Rayet NC Star',
		Description: 'Wolf-Rayet class stars are massive stars that are nearing the end of their life cycle and have moved out of their hydrogen-burning phase. They were once over 20 solar masses but now shed considerable amounts of material through solar wind. Their surface temperature can reach 200,000 K, so they appear a brilliant blue. This star is a variant of the Wolf-Rayet stars whose spectrum is dominated by ionised nitrogen, carbon-oxygen and helium lines.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassWC',
		Short: 'Wolf-Rayet C Star',
		Description: 'Wolf-Rayet class stars are massive stars that are nearing the end of their life cycle and have moved out of their hydrogen-burning phase. They were once over 20 solar masses but now shed considerable amounts of material through solar wind. Their surface temperature can reach 200,000 K, so they appear a brilliant blue. This star is a variant of the Wolf-Rayet stars whose spectrum is dominated by ionised carbon lines.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassWO',
		Short: 'Wolf-Rayet O Star',
		Description: 'Wolf-Rayet class stars are massive stars that are nearing the end of their life cycle and have moved out of their hydrogen-burning phase. They were once over 20 solar masses but now shed considerable amounts of material through solar wind. Their surface temperature can reach 200,000 K, so they appear a brilliant blue. This star is a variant of the Wolf-Rayet stars whose spectrum is dominated by ionised oxygen lines.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassGS',
		Short: 'Class S Giant Star',
		Description: 'Class S stars are a late-type giant star (similar to class K5–M) whose spectrum displays bands from zirconium oxide, in addition to the titanium oxide bands characteristically exhibited by K and M class giant stars.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassS',
		Short: 'Class S Star',
		Description: 'S class stars are late sequence stars that were once M class stars and have begun the cycle towards becoming a carbon star, a star nearing the end of its life.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassMS',
		Short: 'Class MS Star',
		Description: 'MS class stars are late sequence stars having progressed from the S class stage of their life cycle and heading towards becoming a carbon star, a star nearing the end of its stellar life.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassC',
		Short: 'Carbon Star',
		Description: 'Carbon class stars are stars approaching the end of their life. A carbon star is a late-type star similar to a red giant (or occasionally to a red dwarf) whose atmosphere contains more carbon than oxygen; the two elements combine in the upper layers of the star, forming carbon monoxide, which consumes all the oxygen in the atmosphere, leaving carbon atoms free to form other carbon compounds, giving the star a \'sooty\' atmosphere and a strikingly ruby red appearance. The surface temperature is rarely higher than 4300 K.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassCN',
		Short: 'Carbon C-N Star',
		Description: 'Class C-N stars are variants of carbon class stars, stars that are approaching the end of their stellar lives as hydrogen fusion begins to stop. They were once K or M type stars that have degenerated to the carbon phase of their life cycle.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassDA',
		Short: 'Class DA White Dwarf',
		Description: 'White Dwarf stars are stellar remnants. Nuclear fusion has now ceased, and in the absence of radiation pressure the core has collapsed to a tiny fraction of the diameter of the original star, heating it up greatly before it begins its slow cooling down phase. Surface temperatures are usually between 8,000 K and 40,000K so these stellar remnants are blue-white. Class DA stars are white dwarf stars with a hydrogen rich atmosphere.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassDB',
		Short: 'Class DB White Dwarf',
		Description: 'White Dwarf stars are stellar remnants. Nuclear fusion has now ceased, and in the absence of radiation pressure the core has collapsed to a tiny fraction of the diameter of the original star, heating it up greatly before it begins its slow cooling down phase. Surface temperatures are usually between 8,000K and 40,000K so these stellar remnants are blue-white. Class DB stars are white dwarf stars with a helium rich atmosphere with neutral helium emission lines.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassDAB',
		Short: 'Class DAB White Dwarf',
		Description: 'White Dwarf stars are stellar remnants. Nuclear fusion has now ceased, and in the absence of radiation pressure the core has collapsed to a tiny fraction of the diameter of the original star, heating it up greatly before it begins its slow cooling down phase. Surface temperatures are usually between 8,000K and 40,000K so these stellar remnants are blue-white. Class DAB stars are white dwarf stars with hydrogen and helium rich atmospheres and neutral helium emission lines.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassDC',
		Short: 'Class DC White Dwarf',
		Description: 'White Dwarf stars are stellar remnants. Nuclear fusion has now ceased, and in the absence of radiation pressure the core has collapsed to a tiny fraction of the diameter of the original star, heating it up greatly before it begins its slow cooling down phase. Surface temperatures are usually between 8,000K and 40,000K so these stellar remnants are blue-white.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassDCV',
		Short: 'Class DC Variable White Dwarf',
		Description: 'White Dwarf stars are stellar remnants. Nuclear fusion has now ceased, and in the absence of radiation pressure the core has collapsed to a tiny fraction of the diameter of the original star, heating it up greatly before it begins its slow cooling down phase. Surface temperatures are usually between 8,000K and 40,000K so these stellar remnants are blue-white. Class DCV stars are white dwarfs with varying luminosity.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassN',
		Short: 'Neutron Star',
		Description: 'Neutron stars are the stellar remnants of massive stars that have reached the end of their lives. Once nuclear fusion was exhausted, the star collapsed into a tiny volume. Because of its high mass, the matter has collapsed into an extremely high density state made up entirely of neutrons, though with insufficient mass to become a black hole.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassBH',
		Short: 'Black Hole',
		Description: 'Black holes are typically the stellar remnants of super massive stars of twenty solar masses or more, that have reached the end of their lives. Nuclear fusion has ceased, and the star collapsed to the most extreme point possible - where gravity was so extreme light can no longer escape. If matter should fall on to such a body, an extreme burst of gamma radiation will be emitted, but otherwise the body is only visible by the gravitational distortion in the vicinity. In many cases black holes can be seen emitting brightly in X-rays because of matter falling on to their surface from a companion body.',
		Planet: false,
		Star: true

    },
	{
            Name: 'ClassSB',
		Short: 'Supermassive Black Hole',
		Description: 'Super massive black holes tend to form when an initial black hole begins to swallow ever more mass, including other black holes. With time they acquire a vast mass and become a key component of the galaxy, with much of the other galactic mass rotating around them, and can be several million solar masses.',
		Planet: false,
		Star: true

    },
	{
            Name: 'Giant1',
		Short: 'Class I Gas Giant',
		Description: 'Class I or jovian gas giants have primarily hydrogen and helium atmospheres. Coloration comes from clouds in the upper atmosphere of ammonia, water vapour, hydrogen sulphide, phosphine and sulphur. The temperature at the top of their upper cloud layers is typically less than 150 K.',
		Planet: true,
		Star: false

    },
	{
            Name: 'Giant2',
		Short: 'Class II Gas Giant',
		Description: 'Class II gas giants have primarily hydrogen and helium atmospheres. Water vapour in the upper cloud layers gives them a much higher albedo. Their surface temperature is typically up to or around 250 K.',
		Planet: true,
		Star: false

    },
	{
            Name: 'Giant3',
		Short: 'Class III Gas Giant',
		Description: 'Class III gas giants have primarily hydrogen and helium atmospheres without distinctive cloud layers. Their surface temperature typically ranges between 350 K and 800 K. They are primarily blue in colour because of optical scattering in the atmosphere - with the chance of wispy cloud layers from sulphides and chlorides.',
		Planet: true,
		Star: false

    },
	{
            Name: 'Giant4',
		Short: 'Class IV Gas Giant',
		Description: 'Class IV gas giants have primarily hydrogen and helium atmospheres with carbon monoxide and upper clouds of alkali metals above lower cloud layers of silicates and iron compounds, hence he brighter colours. The temperature of their upper cloud layers is typically above 900 K.',
		Planet: true,
		Star: false

    },
	{
            Name: 'Giant5',
		Short: 'Class V Gas Giant',
		Description: 'Class V gas giants have primarily hydrogen and helium atmospheres, with thick clouds of silicates and iron compounds, even metallic iron.They are the hottest type of gas giants with temperatures at their upper cloud decks above 1400 K, and much hotter in the lower layers, often emitting a dull glow from the internal heat within.',
		Planet: true,
		Star: false

    },
	{
            Name: 'GiantAL',
		Short: 'Gas Giant with Ammonia-Based Life',
		Description: 'Gas giant with ammonia-based life. This is primarily a hydrogen and helium-based atmospheric gas giant, but a little below the surface cloud layers, life exists based in the ammonia-cloud layer. The chemistry of this gaseous region is far from equilibrium, with a surprising excess of oxygen and many carbon-based compounds giving it some vivid colours. As with many such gaseous living systems, it is underpinned by vast quantities of free-floating radioplankton - tiny carbon-based algae, each retaining small quantity of liquid ammonia, extracting their energy from the intense radiation flux.',
		Planet: true,
		Star: false

    },
	{
            Name: 'GiantWL',
		Short: 'Gas Giant with Water-Based Life',
		Description: 'Gas giant with water-based life. This is primarily a hydrogen and helium based atmospheric gas giant, but not far below the surface exists life based in the water-cloud layer just below the atmospheric surface. The chemistry of this gaseous region is far from equilibrium, with a surprising excess of oxygen and many carbon-based compounds giving it some vivid colours. As with many such gaseous living systems, it is underpinned by vast quantities of free-floating radioplankton - tiny carbon-based algae, each retaining small quantity of liquid water, extracting their energy from the intense radiation flux.',
		Planet: true,
		Star: false

    },
	{
            Name: 'GiantH',
		Short: 'Helium-Rich Gas Giant',
		Description: 'Helium-rich gas giants have a greatly inflated percentage of helium compared to the hydrogen in their atmosphere. Much of their hydrogen has been lost over time because they have insufficient mass to hold on to it. It may also be because temperatures in their past were much higher, driving off the hydrogen at a greater rate.',
		Planet: true,
		Star: false

    },
	{
            Name: 'GiantW',
		Short: 'Water Giant',
		Description: 'Water giant. Worlds like this have a large atmosphere made mainly of water vapour. It most likely formed when a large icy body warmed up enough to evaporate a large amount of its surface ice. This would in turn trigger a runaway greenhouse effect leading to a very thick atmosphere made of the evaporated ices.',
		Planet: true,
		Star: false

    },
	{
            Name: 'PlanetMR',
		Short: 'Metal-Rich World',
		Description: 'Metal rich worlds like this have a large metallic core, with plentiful metallic ores even at the surface. In places, especially around areas of past or current volcanism or liquid erosion, some higher metals can be found in their elemental form too. Mining is therefore very efficient, so these worlds are highly valued.',
		Planet: true,
		Star: false

    },
	{
            Name: 'PlanetM',
		Short: 'High Metal Content World',
		Description: 'High metal content world with a metallic core. Worlds like this can have metallic ores near the surface in places, especially around areas of past volcanism.',
		Planet: true,
		Star: false

    },
	{
            Name: 'PlanetR',
		Short: 'Rocky World',
		Description: 'Rocky world with little or no surface metal content. Worlds like this have lost most of their volatiles due to past heating, and any metallic content will form a small central core.',
		Planet: true,
		Star: false

    },
	{
            Name: 'PlanetRI',
		Short: 'Rocky Ice World',
		Description: 'Rocky ice world. Typically formed in the cooler regions of a star system these worlds have a small metal core and thick rocky mantle with a crust of very deep ice. Geological activity is common in these worlds because of the large quantities of volatiles in the crust, often creating a thin, sometimes seasonal atmosphere.',
		Planet: true,
		Star: false

    },
	{
            Name: 'PlanetI',
		Short: 'Ice World',
		Description: 'Ice world composed mainly of water ice. Worlds like this will not have had much heating in the past, forming in the cooler regions of a star system and have retained many volatiles as solids in their crust.',
		Planet: true,
		Star: false

    },
	{
            Name: 'PlanetT',
		Short: 'Earth-Like World',
		Description: 'Outdoor world with a human-breathable atmosphere and indigenous life. The atmosphere is far from chemical equilibrium as a result.',
		Planet: true,
		Star: false

    },
	{
            Name: 'PlanetW',
		Short: 'Water World',
		Description: 'Terrestrial water world with an active water-based chemistry and carbon-water-based life.',
		Planet: true,
		Star: false

    },
	{
            Name: 'PlanetA',
		Short: 'Ammonia World',
		Description: 'Terrestrial ammonia world with an active ammonia-based chemistry and carbon-ammonia based life.',
		Planet: true,
		Star: false

    }
]
}";

            JObject jo = JObject.Parse(json);
        }


}
}
