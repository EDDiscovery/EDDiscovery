
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Glut
{

	public class GLUTStrokeRoman
	{

		static CoordRec[] char33_stroke0 = {
			new CoordRec(13.3819F, 100F),
			new CoordRec(13.3819F, 33.3333F)

		};
		static CoordRec[] char33_stroke1 = {
			new CoordRec(13.3819F, 9.5238F),
			new CoordRec(8.62F, 4.7619F),
			new CoordRec(13.3819F, 0F),
			new CoordRec(18.1438F, 4.7619F),
			new CoordRec(13.3819F, 9.5238F)

		};
		static StrokeRec[] char33 = {
			new StrokeRec(2, char33_stroke0),
			new StrokeRec(5, char33_stroke1)

		};
		// char: 34 '"' */

		static CoordRec[] char34_stroke0 = {
			new CoordRec(4.02F, 100F),
			new CoordRec(4.02F, 66.6667F)

		};
		static CoordRec[] char34_stroke1 = {
			new CoordRec(42.1152F, 100F),
			new CoordRec(42.1152F, 66.6667F)

		};
		static StrokeRec[] char34 = {
			new StrokeRec(2, char34_stroke0),
			new StrokeRec(2, char34_stroke1)

		};
		// char: 35 '#' */

		static CoordRec[] char35_stroke0 = {
			new CoordRec(41.2952F, 119.048F),
			new CoordRec(7.9619F, -33.3333F)

		};
		static CoordRec[] char35_stroke1 = {
			new CoordRec(69.8667F, 119.048F),
			new CoordRec(36.5333F, -33.3333F)

		};
		static CoordRec[] char35_stroke2 = {
			new CoordRec(7.9619F, 57.1429F),
			new CoordRec(74.6286F, 57.1429F)

		};
		static CoordRec[] char35_stroke3 = {
			new CoordRec(3.2F, 28.5714F),
			new CoordRec(69.8667F, 28.5714F)

		};
		static StrokeRec[] char35 = {
			new StrokeRec(2, char35_stroke0),
			new StrokeRec(2, char35_stroke1),
			new StrokeRec(2, char35_stroke2),
			new StrokeRec(2, char35_stroke3)

		};
		// char: 36 '$' */

		static CoordRec[] char36_stroke0 = {
			new CoordRec(28.6295F, 119.048F),
			new CoordRec(28.6295F, -19.0476F)

		};
		static CoordRec[] char36_stroke1 = {
			new CoordRec(47.6771F, 119.048F),
			new CoordRec(47.6771F, -19.0476F)

		};
		static CoordRec[] char36_stroke2 = {
			new CoordRec(71.4867F, 85.7143F),
			new CoordRec(61.9629F, 95.2381F),
			new CoordRec(47.6771F, 100F),
			new CoordRec(28.6295F, 100F),
			new CoordRec(14.3438F, 95.2381F),
			new CoordRec(4.82F, 85.7143F),
			new CoordRec(4.82F, 76.1905F),
			new CoordRec(9.5819F, 66.6667F),
			new CoordRec(14.3438F, 61.9048F),
			new CoordRec(23.8676F, 57.1429F),
			new CoordRec(52.439F, 47.619F),
			new CoordRec(61.9629F, 42.8571F),
			new CoordRec(66.7248F, 38.0952F),
			new CoordRec(71.4867F, 28.5714F),
			new CoordRec(71.4867F, 14.2857F),
			new CoordRec(61.9629F, 4.7619F),
			new CoordRec(47.6771F, 0F),
			new CoordRec(28.6295F, 0F),
			new CoordRec(14.3438F, 4.7619F),
			new CoordRec(4.82F, 14.2857F)

		};
		static StrokeRec[] char36 = {
			new StrokeRec(2, char36_stroke0),
			new StrokeRec(2, char36_stroke1),
			new StrokeRec(20, char36_stroke2)

		};
		// char: 37 '%' */

		static CoordRec[] char37_stroke0 = {
			new CoordRec(92.0743F, 100F),
			new CoordRec(6.36F, 0F)

		};
		static CoordRec[] char37_stroke1 = {
			new CoordRec(30.1695F, 100F),
			new CoordRec(39.6933F, 90.4762F),
			new CoordRec(39.6933F, 80.9524F),
			new CoordRec(34.9314F, 71.4286F),
			new CoordRec(25.4076F, 66.6667F),
			new CoordRec(15.8838F, 66.6667F),
			new CoordRec(6.36F, 76.1905F),
			new CoordRec(6.36F, 85.7143F),
			new CoordRec(11.1219F, 95.2381F),
			new CoordRec(20.6457F, 100F),
			new CoordRec(30.1695F, 100F),
			new CoordRec(39.6933F, 95.2381F),
			new CoordRec(53.979F, 90.4762F),
			new CoordRec(68.2648F, 90.4762F),
			new CoordRec(82.5505F, 95.2381F),
			new CoordRec(92.0743F, 100F)

		};
		static CoordRec[] char37_stroke2 = {
			new CoordRec(73.0267F, 33.3333F),
			new CoordRec(63.5029F, 28.5714F),
			new CoordRec(58.741F, 19.0476F),
			new CoordRec(58.741F, 9.5238F),
			new CoordRec(68.2648F, 0F),
			new CoordRec(77.7886F, 0F),
			new CoordRec(87.3124F, 4.7619F),
			new CoordRec(92.0743F, 14.2857F),
			new CoordRec(92.0743F, 23.8095F),
			new CoordRec(82.5505F, 33.3333F),
			new CoordRec(73.0267F, 33.3333F)

		};
		static StrokeRec[] char37 = {
			new StrokeRec(2, char37_stroke0),
			new StrokeRec(16, char37_stroke1),
			new StrokeRec(11, char37_stroke2)

		};
		// char: 38 '&' */

		static CoordRec[] char38_stroke0 = {
			new CoordRec(101.218F, 57.1429F),
			new CoordRec(101.218F, 61.9048F),
			new CoordRec(96.4562F, 66.6667F),
			new CoordRec(91.6943F, 66.6667F),
			new CoordRec(86.9324F, 61.9048F),
			new CoordRec(82.1705F, 52.381F),
			new CoordRec(72.6467F, 28.5714F),
			new CoordRec(63.1229F, 14.2857F),
			new CoordRec(53.599F, 4.7619F),
			new CoordRec(44.0752F, 0F),
			new CoordRec(25.0276F, 0F),
			new CoordRec(15.5038F, 4.7619F),
			new CoordRec(10.7419F, 9.5238F),
			new CoordRec(5.98F, 19.0476F),
			new CoordRec(5.98F, 28.5714F),
			new CoordRec(10.7419F, 38.0952F),
			new CoordRec(15.5038F, 42.8571F),
			new CoordRec(48.8371F, 61.9048F),
			new CoordRec(53.599F, 66.6667F),
			new CoordRec(58.361F, 76.1905F),
			new CoordRec(58.361F, 85.7143F),
			new CoordRec(53.599F, 95.2381F),
			new CoordRec(44.0752F, 100F),
			new CoordRec(34.5514F, 95.2381F),
			new CoordRec(29.7895F, 85.7143F),
			new CoordRec(29.7895F, 76.1905F),
			new CoordRec(34.5514F, 61.9048F),
			new CoordRec(44.0752F, 47.619F),
			new CoordRec(67.8848F, 14.2857F),
			new CoordRec(77.4086F, 4.7619F),
			new CoordRec(86.9324F, 0F),
			new CoordRec(96.4562F, 0F),
			new CoordRec(101.218F, 4.7619F),
			new CoordRec(101.218F, 9.5238F)

		};

		static StrokeRec[] char38 = { new StrokeRec(34, char38_stroke0) };
		// char: 39 ''' */

		static CoordRec[] char39_stroke0 = {
			new CoordRec(4.44F, 100F),
			new CoordRec(4.44F, 66.6667F)

		};

		static StrokeRec[] char39 = { new StrokeRec(2, char39_stroke0) };
		// char: 40 '(' */

		static CoordRec[] char40_stroke0 = {
			new CoordRec(40.9133F, 119.048F),
			new CoordRec(31.3895F, 109.524F),
			new CoordRec(21.8657F, 95.2381F),
			new CoordRec(12.3419F, 76.1905F),
			new CoordRec(7.58F, 52.381F),
			new CoordRec(7.58F, 33.3333F),
			new CoordRec(12.3419F, 9.5238F),
			new CoordRec(21.8657F, -9.5238F),
			new CoordRec(31.3895F, -23.8095F),
			new CoordRec(40.9133F, -33.3333F)

		};

		static StrokeRec[] char40 = { new StrokeRec(10, char40_stroke0) };
		// char: 41 ')' */

		static CoordRec[] char41_stroke0 = {
			new CoordRec(5.28F, 119.048F),
			new CoordRec(14.8038F, 109.524F),
			new CoordRec(24.3276F, 95.2381F),
			new CoordRec(33.8514F, 76.1905F),
			new CoordRec(38.6133F, 52.381F),
			new CoordRec(38.6133F, 33.3333F),
			new CoordRec(33.8514F, 9.5238F),
			new CoordRec(24.3276F, -9.5238F),
			new CoordRec(14.8038F, -23.8095F),
			new CoordRec(5.28F, -33.3333F)

		};

		static StrokeRec[] char41 = { new StrokeRec(10, char41_stroke0) };
		// char: 42 '*' */

		static CoordRec[] char42_stroke0 = {
			new CoordRec(30.7695F, 71.4286F),
			new CoordRec(30.7695F, 14.2857F)

		};
		static CoordRec[] char42_stroke1 = {
			new CoordRec(6.96F, 57.1429F),
			new CoordRec(54.579F, 28.5714F)

		};
		static CoordRec[] char42_stroke2 = {
			new CoordRec(54.579F, 57.1429F),
			new CoordRec(6.96F, 28.5714F)

		};
		static StrokeRec[] char42 = {
			new StrokeRec(2, char42_stroke0),
			new StrokeRec(2, char42_stroke1),
			new StrokeRec(2, char42_stroke2)

		};
		// char: 43 '+' */

		static CoordRec[] char43_stroke0 = {
			new CoordRec(48.8371F, 85.7143F),
			new CoordRec(48.8371F, 0F)

		};
		static CoordRec[] char43_stroke1 = {
			new CoordRec(5.98F, 42.8571F),
			new CoordRec(91.6943F, 42.8571F)

		};
		static StrokeRec[] char43 = {
			new StrokeRec(2, char43_stroke0),
			new StrokeRec(2, char43_stroke1)

		};
		// char: 44 ',' */

		static CoordRec[] char44_stroke0 = {
			new CoordRec(18.2838F, 4.7619F),
			new CoordRec(13.5219F, 0F),
			new CoordRec(8.76F, 4.7619F),
			new CoordRec(13.5219F, 9.5238F),
			new CoordRec(18.2838F, 4.7619F),
			new CoordRec(18.2838F, -4.7619F),
			new CoordRec(13.5219F, -14.2857F),
			new CoordRec(8.76F, -19.0476F)

		};

		static StrokeRec[] char44 = { new StrokeRec(8, char44_stroke0) };
		// char: 45 '-' */

		static CoordRec[] char45_stroke0 = {
			new CoordRec(7.38F, 42.8571F),
			new CoordRec(93.0943F, 42.8571F)

		};

		static StrokeRec[] char45 = { new StrokeRec(2, char45_stroke0) };
		// char: 46 '.' */

		static CoordRec[] char46_stroke0 = {
			new CoordRec(13.1019F, 9.5238F),
			new CoordRec(8.34F, 4.7619F),
			new CoordRec(13.1019F, 0F),
			new CoordRec(17.8638F, 4.7619F),
			new CoordRec(13.1019F, 9.5238F)

		};

		static StrokeRec[] char46 = { new StrokeRec(5, char46_stroke0) };
		// char: 47 '/' */

		static CoordRec[] char47_stroke0 = {
			new CoordRec(7.24F, -14.2857F),
			new CoordRec(73.9067F, 100F)

		};

		static StrokeRec[] char47 = { new StrokeRec(2, char47_stroke0) };
		// char: 48 '0' */

		static CoordRec[] char48_stroke0 = {
			new CoordRec(33.5514F, 100F),
			new CoordRec(19.2657F, 95.2381F),
			new CoordRec(9.7419F, 80.9524F),
			new CoordRec(4.98F, 57.1429F),
			new CoordRec(4.98F, 42.8571F),
			new CoordRec(9.7419F, 19.0476F),
			new CoordRec(19.2657F, 4.7619F),
			new CoordRec(33.5514F, 0F),
			new CoordRec(43.0752F, 0F),
			new CoordRec(57.361F, 4.7619F),
			new CoordRec(66.8848F, 19.0476F),
			new CoordRec(71.6467F, 42.8571F),
			new CoordRec(71.6467F, 57.1429F),
			new CoordRec(66.8848F, 80.9524F),
			new CoordRec(57.361F, 95.2381F),
			new CoordRec(43.0752F, 100F),
			new CoordRec(33.5514F, 100F)

		};

		static StrokeRec[] char48 = { new StrokeRec(17, char48_stroke0) };
		// char: 49 '1' */

		static CoordRec[] char49_stroke0 = {
			new CoordRec(11.82F, 80.9524F),
			new CoordRec(21.3438F, 85.7143F),
			new CoordRec(35.6295F, 100F),
			new CoordRec(35.6295F, 0F)

		};

		static StrokeRec[] char49 = { new StrokeRec(4, char49_stroke0) };
		// char: 50 '2' */

		static CoordRec[] char50_stroke0 = {
			new CoordRec(10.1819F, 76.1905F),
			new CoordRec(10.1819F, 80.9524F),
			new CoordRec(14.9438F, 90.4762F),
			new CoordRec(19.7057F, 95.2381F),
			new CoordRec(29.2295F, 100F),
			new CoordRec(48.2771F, 100F),
			new CoordRec(57.801F, 95.2381F),
			new CoordRec(62.5629F, 90.4762F),
			new CoordRec(67.3248F, 80.9524F),
			new CoordRec(67.3248F, 71.4286F),
			new CoordRec(62.5629F, 61.9048F),
			new CoordRec(53.039F, 47.619F),
			new CoordRec(5.42F, 0F),
			new CoordRec(72.0867F, 0F)

		};

		static StrokeRec[] char50 = { new StrokeRec(14, char50_stroke0) };
		// char: 51 '3' */

		static CoordRec[] char51_stroke0 = {
			new CoordRec(14.5238F, 100F),
			new CoordRec(66.9048F, 100F),
			new CoordRec(38.3333F, 61.9048F),
			new CoordRec(52.619F, 61.9048F),
			new CoordRec(62.1429F, 57.1429F),
			new CoordRec(66.9048F, 52.381F),
			new CoordRec(71.6667F, 38.0952F),
			new CoordRec(71.6667F, 28.5714F),
			new CoordRec(66.9048F, 14.2857F),
			new CoordRec(57.381F, 4.7619F),
			new CoordRec(43.0952F, 0F),
			new CoordRec(28.8095F, 0F),
			new CoordRec(14.5238F, 4.7619F),
			new CoordRec(9.7619F, 9.5238F),
			new CoordRec(5F, 19.0476F)

		};

		static StrokeRec[] char51 = { new StrokeRec(15, char51_stroke0) };
		// char: 52 '4' */

		static CoordRec[] char52_stroke0 = {
			new CoordRec(51.499F, 100F),
			new CoordRec(3.88F, 33.3333F),
			new CoordRec(75.3086F, 33.3333F)

		};
		static CoordRec[] char52_stroke1 = {
			new CoordRec(51.499F, 100F),
			new CoordRec(51.499F, 0F)

		};
		static StrokeRec[] char52 = {
			new StrokeRec(3, char52_stroke0),
			new StrokeRec(2, char52_stroke1)

		};
		// char: 53 '5' */

		static CoordRec[] char53_stroke0 = {
			new CoordRec(62.0029F, 100F),
			new CoordRec(14.3838F, 100F),
			new CoordRec(9.6219F, 57.1429F),
			new CoordRec(14.3838F, 61.9048F),
			new CoordRec(28.6695F, 66.6667F),
			new CoordRec(42.9552F, 66.6667F),
			new CoordRec(57.241F, 61.9048F),
			new CoordRec(66.7648F, 52.381F),
			new CoordRec(71.5267F, 38.0952F),
			new CoordRec(71.5267F, 28.5714F),
			new CoordRec(66.7648F, 14.2857F),
			new CoordRec(57.241F, 4.7619F),
			new CoordRec(42.9552F, 0F),
			new CoordRec(28.6695F, 0F),
			new CoordRec(14.3838F, 4.7619F),
			new CoordRec(9.6219F, 9.5238F),
			new CoordRec(4.86F, 19.0476F)

		};

		static StrokeRec[] char53 = { new StrokeRec(17, char53_stroke0) };
		// char: 54 '6' */

		static CoordRec[] char54_stroke0 = {
			new CoordRec(62.7229F, 85.7143F),
			new CoordRec(57.961F, 95.2381F),
			new CoordRec(43.6752F, 100F),
			new CoordRec(34.1514F, 100F),
			new CoordRec(19.8657F, 95.2381F),
			new CoordRec(10.3419F, 80.9524F),
			new CoordRec(5.58F, 57.1429F),
			new CoordRec(5.58F, 33.3333F),
			new CoordRec(10.3419F, 14.2857F),
			new CoordRec(19.8657F, 4.7619F),
			new CoordRec(34.1514F, 0F),
			new CoordRec(38.9133F, 0F),
			new CoordRec(53.199F, 4.7619F),
			new CoordRec(62.7229F, 14.2857F),
			new CoordRec(67.4848F, 28.5714F),
			new CoordRec(67.4848F, 33.3333F),
			new CoordRec(62.7229F, 47.619F),
			new CoordRec(53.199F, 57.1429F),
			new CoordRec(38.9133F, 61.9048F),
			new CoordRec(34.1514F, 61.9048F),
			new CoordRec(19.8657F, 57.1429F),
			new CoordRec(10.3419F, 47.619F),
			new CoordRec(5.58F, 33.3333F)

		};

		static StrokeRec[] char54 = { new StrokeRec(23, char54_stroke0) };
		// char: 55 '7' */

		static CoordRec[] char55_stroke0 = {
			new CoordRec(72.2267F, 100F),
			new CoordRec(24.6076F, 0F)

		};
		static CoordRec[] char55_stroke1 = {
			new CoordRec(5.56F, 100F),
			new CoordRec(72.2267F, 100F)

		};
		static StrokeRec[] char55 = {
			new StrokeRec(2, char55_stroke0),
			new StrokeRec(2, char55_stroke1)

		};
		// char: 56 '8' */

		static CoordRec[] char56_stroke0 = {
			new CoordRec(29.4095F, 100F),
			new CoordRec(15.1238F, 95.2381F),
			new CoordRec(10.3619F, 85.7143F),
			new CoordRec(10.3619F, 76.1905F),
			new CoordRec(15.1238F, 66.6667F),
			new CoordRec(24.6476F, 61.9048F),
			new CoordRec(43.6952F, 57.1429F),
			new CoordRec(57.981F, 52.381F),
			new CoordRec(67.5048F, 42.8571F),
			new CoordRec(72.2667F, 33.3333F),
			new CoordRec(72.2667F, 19.0476F),
			new CoordRec(67.5048F, 9.5238F),
			new CoordRec(62.7429F, 4.7619F),
			new CoordRec(48.4571F, 0F),
			new CoordRec(29.4095F, 0F),
			new CoordRec(15.1238F, 4.7619F),
			new CoordRec(10.3619F, 9.5238F),
			new CoordRec(5.6F, 19.0476F),
			new CoordRec(5.6F, 33.3333F),
			new CoordRec(10.3619F, 42.8571F),
			new CoordRec(19.8857F, 52.381F),
			new CoordRec(34.1714F, 57.1429F),
			new CoordRec(53.219F, 61.9048F),
			new CoordRec(62.7429F, 66.6667F),
			new CoordRec(67.5048F, 76.1905F),
			new CoordRec(67.5048F, 85.7143F),
			new CoordRec(62.7429F, 95.2381F),
			new CoordRec(48.4571F, 100F),
			new CoordRec(29.4095F, 100F)

		};

		static StrokeRec[] char56 = { new StrokeRec(29, char56_stroke0) };
		// char: 57 '9' */

		static CoordRec[] char57_stroke0 = {
			new CoordRec(68.5048F, 66.6667F),
			new CoordRec(63.7429F, 52.381F),
			new CoordRec(54.219F, 42.8571F),
			new CoordRec(39.9333F, 38.0952F),
			new CoordRec(35.1714F, 38.0952F),
			new CoordRec(20.8857F, 42.8571F),
			new CoordRec(11.3619F, 52.381F),
			new CoordRec(6.6F, 66.6667F),
			new CoordRec(6.6F, 71.4286F),
			new CoordRec(11.3619F, 85.7143F),
			new CoordRec(20.8857F, 95.2381F),
			new CoordRec(35.1714F, 100F),
			new CoordRec(39.9333F, 100F),
			new CoordRec(54.219F, 95.2381F),
			new CoordRec(63.7429F, 85.7143F),
			new CoordRec(68.5048F, 66.6667F),
			new CoordRec(68.5048F, 42.8571F),
			new CoordRec(63.7429F, 19.0476F),
			new CoordRec(54.219F, 4.7619F),
			new CoordRec(39.9333F, 0F),
			new CoordRec(30.4095F, 0F),
			new CoordRec(16.1238F, 4.7619F),
			new CoordRec(11.3619F, 14.2857F)

		};

		static StrokeRec[] char57 = { new StrokeRec(23, char57_stroke0) };
		// char: 58 ':' */

		static CoordRec[] char58_stroke0 = {
			new CoordRec(14.0819F, 66.6667F),
			new CoordRec(9.32F, 61.9048F),
			new CoordRec(14.0819F, 57.1429F),
			new CoordRec(18.8438F, 61.9048F),
			new CoordRec(14.0819F, 66.6667F)

		};
		static CoordRec[] char58_stroke1 = {
			new CoordRec(14.0819F, 9.5238F),
			new CoordRec(9.32F, 4.7619F),
			new CoordRec(14.0819F, 0F),
			new CoordRec(18.8438F, 4.7619F),
			new CoordRec(14.0819F, 9.5238F)

		};
		static StrokeRec[] char58 = {
			new StrokeRec(5, char58_stroke0),
			new StrokeRec(5, char58_stroke1)

		};
		// char: 59 ';' */

		static CoordRec[] char59_stroke0 = {
			new CoordRec(12.9619F, 66.6667F),
			new CoordRec(8.2F, 61.9048F),
			new CoordRec(12.9619F, 57.1429F),
			new CoordRec(17.7238F, 61.9048F),
			new CoordRec(12.9619F, 66.6667F)

		};
		static CoordRec[] char59_stroke1 = {
			new CoordRec(17.7238F, 4.7619F),
			new CoordRec(12.9619F, 0F),
			new CoordRec(8.2F, 4.7619F),
			new CoordRec(12.9619F, 9.5238F),
			new CoordRec(17.7238F, 4.7619F),
			new CoordRec(17.7238F, -4.7619F),
			new CoordRec(12.9619F, -14.2857F),
			new CoordRec(8.2F, -19.0476F)

		};
		static StrokeRec[] char59 = {
			new StrokeRec(5, char59_stroke0),
			new StrokeRec(8, char59_stroke1)

		};
		// char: 60 '<' */

		static CoordRec[] char60_stroke0 = {
			new CoordRec(79.2505F, 85.7143F),
			new CoordRec(3.06F, 42.8571F),
			new CoordRec(79.2505F, 0F)

		};

		static StrokeRec[] char60 = { new StrokeRec(3, char60_stroke0) };
		// char: 61 '=' */

		static CoordRec[] char61_stroke0 = {
			new CoordRec(5.7F, 57.1429F),
			new CoordRec(91.4143F, 57.1429F)

		};
		static CoordRec[] char61_stroke1 = {
			new CoordRec(5.7F, 28.5714F),
			new CoordRec(91.4143F, 28.5714F)

		};
		static StrokeRec[] char61 = {
			new StrokeRec(2, char61_stroke0),
			new StrokeRec(2, char61_stroke1)

		};
		// char: 62 '>' */

		static CoordRec[] char62_stroke0 = {
			new CoordRec(2.78F, 85.7143F),
			new CoordRec(78.9705F, 42.8571F),
			new CoordRec(2.78F, 0F)

		};

		static StrokeRec[] char62 = { new StrokeRec(3, char62_stroke0) };
		// char: 63 '?' */

		static CoordRec[] char63_stroke0 = {
			new CoordRec(8.42F, 76.1905F),
			new CoordRec(8.42F, 80.9524F),
			new CoordRec(13.1819F, 90.4762F),
			new CoordRec(17.9438F, 95.2381F),
			new CoordRec(27.4676F, 100F),
			new CoordRec(46.5152F, 100F),
			new CoordRec(56.039F, 95.2381F),
			new CoordRec(60.801F, 90.4762F),
			new CoordRec(65.5629F, 80.9524F),
			new CoordRec(65.5629F, 71.4286F),
			new CoordRec(60.801F, 61.9048F),
			new CoordRec(56.039F, 57.1429F),
			new CoordRec(36.9914F, 47.619F),
			new CoordRec(36.9914F, 33.3333F)

		};
		static CoordRec[] char63_stroke1 = {
			new CoordRec(36.9914F, 9.5238F),
			new CoordRec(32.2295F, 4.7619F),
			new CoordRec(36.9914F, 0F),
			new CoordRec(41.7533F, 4.7619F),
			new CoordRec(36.9914F, 9.5238F)

		};
		static StrokeRec[] char63 = {
			new StrokeRec(14, char63_stroke0),
			new StrokeRec(5, char63_stroke1)

		};
		// char: 64 '@' */

		static CoordRec[] char64_stroke0 = {
			new CoordRec(49.2171F, 52.381F),
			new CoordRec(39.6933F, 57.1429F),
			new CoordRec(30.1695F, 57.1429F),
			new CoordRec(25.4076F, 47.619F),
			new CoordRec(25.4076F, 42.8571F),
			new CoordRec(30.1695F, 33.3333F),
			new CoordRec(39.6933F, 33.3333F),
			new CoordRec(49.2171F, 38.0952F)

		};
		static CoordRec[] char64_stroke1 = {
			new CoordRec(49.2171F, 57.1429F),
			new CoordRec(49.2171F, 38.0952F),
			new CoordRec(53.979F, 33.3333F),
			new CoordRec(63.5029F, 33.3333F),
			new CoordRec(68.2648F, 42.8571F),
			new CoordRec(68.2648F, 47.619F),
			new CoordRec(63.5029F, 61.9048F),
			new CoordRec(53.979F, 71.4286F),
			new CoordRec(39.6933F, 76.1905F),
			new CoordRec(34.9314F, 76.1905F),
			new CoordRec(20.6457F, 71.4286F),
			new CoordRec(11.1219F, 61.9048F),
			new CoordRec(6.36F, 47.619F),
			new CoordRec(6.36F, 42.8571F),
			new CoordRec(11.1219F, 28.5714F),
			new CoordRec(20.6457F, 19.0476F),
			new CoordRec(34.9314F, 14.2857F),
			new CoordRec(39.6933F, 14.2857F),
			new CoordRec(53.979F, 19.0476F)

		};
		static StrokeRec[] char64 = {
			new StrokeRec(8, char64_stroke0),
			new StrokeRec(19, char64_stroke1)

		};
		// char: 65 'A' */

		static CoordRec[] char65_stroke0 = {
			new CoordRec(40.5952F, 100F),
			new CoordRec(2.5F, 0F)

		};
		static CoordRec[] char65_stroke1 = {
			new CoordRec(40.5952F, 100F),
			new CoordRec(78.6905F, 0F)

		};
		static CoordRec[] char65_stroke2 = {
			new CoordRec(16.7857F, 33.3333F),
			new CoordRec(64.4048F, 33.3333F)

		};
		static StrokeRec[] char65 = {
			new StrokeRec(2, char65_stroke0),
			new StrokeRec(2, char65_stroke1),
			new StrokeRec(2, char65_stroke2)

		};
		// char: 66 'B' */

		static CoordRec[] char66_stroke0 = {
			new CoordRec(11.42F, 100F),
			new CoordRec(11.42F, 0F)

		};
		static CoordRec[] char66_stroke1 = {
			new CoordRec(11.42F, 100F),
			new CoordRec(54.2771F, 100F),
			new CoordRec(68.5629F, 95.2381F),
			new CoordRec(73.3248F, 90.4762F),
			new CoordRec(78.0867F, 80.9524F),
			new CoordRec(78.0867F, 71.4286F),
			new CoordRec(73.3248F, 61.9048F),
			new CoordRec(68.5629F, 57.1429F),
			new CoordRec(54.2771F, 52.381F)

		};
		static CoordRec[] char66_stroke2 = {
			new CoordRec(11.42F, 52.381F),
			new CoordRec(54.2771F, 52.381F),
			new CoordRec(68.5629F, 47.619F),
			new CoordRec(73.3248F, 42.8571F),
			new CoordRec(78.0867F, 33.3333F),
			new CoordRec(78.0867F, 19.0476F),
			new CoordRec(73.3248F, 9.5238F),
			new CoordRec(68.5629F, 4.7619F),
			new CoordRec(54.2771F, 0F),
			new CoordRec(11.42F, 0F)

		};
		static StrokeRec[] char66 = {
			new StrokeRec(2, char66_stroke0),
			new StrokeRec(9, char66_stroke1),
			new StrokeRec(10, char66_stroke2)

		};
		// char: 67 'C' */

		static CoordRec[] char67_stroke0 = {
			new CoordRec(78.0886F, 76.1905F),
			new CoordRec(73.3267F, 85.7143F),
			new CoordRec(63.8029F, 95.2381F),
			new CoordRec(54.279F, 100F),
			new CoordRec(35.2314F, 100F),
			new CoordRec(25.7076F, 95.2381F),
			new CoordRec(16.1838F, 85.7143F),
			new CoordRec(11.4219F, 76.1905F),
			new CoordRec(6.66F, 61.9048F),
			new CoordRec(6.66F, 38.0952F),
			new CoordRec(11.4219F, 23.8095F),
			new CoordRec(16.1838F, 14.2857F),
			new CoordRec(25.7076F, 4.7619F),
			new CoordRec(35.2314F, 0F),
			new CoordRec(54.279F, 0F),
			new CoordRec(63.8029F, 4.7619F),
			new CoordRec(73.3267F, 14.2857F),
			new CoordRec(78.0886F, 23.8095F)

		};

		static StrokeRec[] char67 = { new StrokeRec(18, char67_stroke0) };
		// char: 68 'D' */

		static CoordRec[] char68_stroke0 = {
			new CoordRec(11.96F, 100F),
			new CoordRec(11.96F, 0F)

		};
		static CoordRec[] char68_stroke1 = {
			new CoordRec(11.96F, 100F),
			new CoordRec(45.2933F, 100F),
			new CoordRec(59.579F, 95.2381F),
			new CoordRec(69.1029F, 85.7143F),
			new CoordRec(73.8648F, 76.1905F),
			new CoordRec(78.6267F, 61.9048F),
			new CoordRec(78.6267F, 38.0952F),
			new CoordRec(73.8648F, 23.8095F),
			new CoordRec(69.1029F, 14.2857F),
			new CoordRec(59.579F, 4.7619F),
			new CoordRec(45.2933F, 0F),
			new CoordRec(11.96F, 0F)

		};
		static StrokeRec[] char68 = {
			new StrokeRec(2, char68_stroke0),
			new StrokeRec(12, char68_stroke1)

		};
		// char: 69 'E' */

		static CoordRec[] char69_stroke0 = {
			new CoordRec(11.42F, 100F),
			new CoordRec(11.42F, 0F)

		};
		static CoordRec[] char69_stroke1 = {
			new CoordRec(11.42F, 100F),
			new CoordRec(73.3248F, 100F)

		};
		static CoordRec[] char69_stroke2 = {
			new CoordRec(11.42F, 52.381F),
			new CoordRec(49.5152F, 52.381F)

		};
		static CoordRec[] char69_stroke3 = {
			new CoordRec(11.42F, 0F),
			new CoordRec(73.3248F, 0F)

		};
		static StrokeRec[] char69 = {
			new StrokeRec(2, char69_stroke0),
			new StrokeRec(2, char69_stroke1),
			new StrokeRec(2, char69_stroke2),
			new StrokeRec(2, char69_stroke3)

		};
		// char: 70 'F' */

		static CoordRec[] char70_stroke0 = {
			new CoordRec(11.42F, 100F),
			new CoordRec(11.42F, 0F)

		};
		static CoordRec[] char70_stroke1 = {
			new CoordRec(11.42F, 100F),
			new CoordRec(73.3248F, 100F)

		};
		static CoordRec[] char70_stroke2 = {
			new CoordRec(11.42F, 52.381F),
			new CoordRec(49.5152F, 52.381F)

		};
		static StrokeRec[] char70 = {
			new StrokeRec(2, char70_stroke0),
			new StrokeRec(2, char70_stroke1),
			new StrokeRec(2, char70_stroke2)

		};
		// char: 71 'G' */

		static CoordRec[] char71_stroke0 = {
			new CoordRec(78.4886F, 76.1905F),
			new CoordRec(73.7267F, 85.7143F),
			new CoordRec(64.2029F, 95.2381F),
			new CoordRec(54.679F, 100F),
			new CoordRec(35.6314F, 100F),
			new CoordRec(26.1076F, 95.2381F),
			new CoordRec(16.5838F, 85.7143F),
			new CoordRec(11.8219F, 76.1905F),
			new CoordRec(7.06F, 61.9048F),
			new CoordRec(7.06F, 38.0952F),
			new CoordRec(11.8219F, 23.8095F),
			new CoordRec(16.5838F, 14.2857F),
			new CoordRec(26.1076F, 4.7619F),
			new CoordRec(35.6314F, 0F),
			new CoordRec(54.679F, 0F),
			new CoordRec(64.2029F, 4.7619F),
			new CoordRec(73.7267F, 14.2857F),
			new CoordRec(78.4886F, 23.8095F),
			new CoordRec(78.4886F, 38.0952F)

		};
		static CoordRec[] char71_stroke1 = {
			new CoordRec(54.679F, 38.0952F),
			new CoordRec(78.4886F, 38.0952F)

		};
		static StrokeRec[] char71 = {
			new StrokeRec(19, char71_stroke0),
			new StrokeRec(2, char71_stroke1)

		};
		// char: 72 'H' */

		static CoordRec[] char72_stroke0 = {
			new CoordRec(11.42F, 100F),
			new CoordRec(11.42F, 0F)

		};
		static CoordRec[] char72_stroke1 = {
			new CoordRec(78.0867F, 100F),
			new CoordRec(78.0867F, 0F)

		};
		static CoordRec[] char72_stroke2 = {
			new CoordRec(11.42F, 52.381F),
			new CoordRec(78.0867F, 52.381F)

		};
		static StrokeRec[] char72 = {
			new StrokeRec(2, char72_stroke0),
			new StrokeRec(2, char72_stroke1),
			new StrokeRec(2, char72_stroke2)

		};
		// char: 73 'I' */

		static CoordRec[] char73_stroke0 = {
			new CoordRec(10.86F, 100F),
			new CoordRec(10.86F, 0F)

		};

		static StrokeRec[] char73 = { new StrokeRec(2, char73_stroke0) };
		// char: 74 'J' */

		static CoordRec[] char74_stroke0 = {
			new CoordRec(50.119F, 100F),
			new CoordRec(50.119F, 23.8095F),
			new CoordRec(45.3571F, 9.5238F),
			new CoordRec(40.5952F, 4.7619F),
			new CoordRec(31.0714F, 0F),
			new CoordRec(21.5476F, 0F),
			new CoordRec(12.0238F, 4.7619F),
			new CoordRec(7.2619F, 9.5238F),
			new CoordRec(2.5F, 23.8095F),
			new CoordRec(2.5F, 33.3333F)

		};

		static StrokeRec[] char74 = { new StrokeRec(10, char74_stroke0) };
		// char: 75 'K' */

		static CoordRec[] char75_stroke0 = {
			new CoordRec(11.28F, 100F),
			new CoordRec(11.28F, 0F)

		};
		static CoordRec[] char75_stroke1 = {
			new CoordRec(77.9467F, 100F),
			new CoordRec(11.28F, 33.3333F)

		};
		static CoordRec[] char75_stroke2 = {
			new CoordRec(35.0895F, 57.1429F),
			new CoordRec(77.9467F, 0F)

		};
		static StrokeRec[] char75 = {
			new StrokeRec(2, char75_stroke0),
			new StrokeRec(2, char75_stroke1),
			new StrokeRec(2, char75_stroke2)

		};
		// char: 76 'L' */

		static CoordRec[] char76_stroke0 = {
			new CoordRec(11.68F, 100F),
			new CoordRec(11.68F, 0F)

		};
		static CoordRec[] char76_stroke1 = {
			new CoordRec(11.68F, 0F),
			new CoordRec(68.8229F, 0F)

		};
		static StrokeRec[] char76 = {
			new StrokeRec(2, char76_stroke0),
			new StrokeRec(2, char76_stroke1)

		};
		// char: 77 'M' */

		static CoordRec[] char77_stroke0 = {
			new CoordRec(10.86F, 100F),
			new CoordRec(10.86F, 0F)

		};
		static CoordRec[] char77_stroke1 = {
			new CoordRec(10.86F, 100F),
			new CoordRec(48.9552F, 0F)

		};
		static CoordRec[] char77_stroke2 = {
			new CoordRec(87.0505F, 100F),
			new CoordRec(48.9552F, 0F)

		};
		static CoordRec[] char77_stroke3 = {
			new CoordRec(87.0505F, 100F),
			new CoordRec(87.0505F, 0F)

		};
		static StrokeRec[] char77 = {
			new StrokeRec(2, char77_stroke0),
			new StrokeRec(2, char77_stroke1),
			new StrokeRec(2, char77_stroke2),
			new StrokeRec(2, char77_stroke3)

		};
		// char: 78 'N' */

		static CoordRec[] char78_stroke0 = {
			new CoordRec(11.14F, 100F),
			new CoordRec(11.14F, 0F)

		};
		static CoordRec[] char78_stroke1 = {
			new CoordRec(11.14F, 100F),
			new CoordRec(77.8067F, 0F)

		};
		static CoordRec[] char78_stroke2 = {
			new CoordRec(77.8067F, 100F),
			new CoordRec(77.8067F, 0F)

		};
		static StrokeRec[] char78 = {
			new StrokeRec(2, char78_stroke0),
			new StrokeRec(2, char78_stroke1),
			new StrokeRec(2, char78_stroke2)

		};
		// char: 79 'O' */

		static CoordRec[] char79_stroke0 = {
			new CoordRec(34.8114F, 100F),
			new CoordRec(25.2876F, 95.2381F),
			new CoordRec(15.7638F, 85.7143F),
			new CoordRec(11.0019F, 76.1905F),
			new CoordRec(6.24F, 61.9048F),
			new CoordRec(6.24F, 38.0952F),
			new CoordRec(11.0019F, 23.8095F),
			new CoordRec(15.7638F, 14.2857F),
			new CoordRec(25.2876F, 4.7619F),
			new CoordRec(34.8114F, 0F),
			new CoordRec(53.859F, 0F),
			new CoordRec(63.3829F, 4.7619F),
			new CoordRec(72.9067F, 14.2857F),
			new CoordRec(77.6686F, 23.8095F),
			new CoordRec(82.4305F, 38.0952F),
			new CoordRec(82.4305F, 61.9048F),
			new CoordRec(77.6686F, 76.1905F),
			new CoordRec(72.9067F, 85.7143F),
			new CoordRec(63.3829F, 95.2381F),
			new CoordRec(53.859F, 100F),
			new CoordRec(34.8114F, 100F)

		};

		static StrokeRec[] char79 = { new StrokeRec(21, char79_stroke0) };
		// char: 80 'P' */

		static CoordRec[] char80_stroke0 = {
			new CoordRec(12.1F, 100F),
			new CoordRec(12.1F, 0F)

		};
		static CoordRec[] char80_stroke1 = {
			new CoordRec(12.1F, 100F),
			new CoordRec(54.9571F, 100F),
			new CoordRec(69.2429F, 95.2381F),
			new CoordRec(74.0048F, 90.4762F),
			new CoordRec(78.7667F, 80.9524F),
			new CoordRec(78.7667F, 66.6667F),
			new CoordRec(74.0048F, 57.1429F),
			new CoordRec(69.2429F, 52.381F),
			new CoordRec(54.9571F, 47.619F),
			new CoordRec(12.1F, 47.619F)

		};
		static StrokeRec[] char80 = {
			new StrokeRec(2, char80_stroke0),
			new StrokeRec(10, char80_stroke1)

		};
		// char: 81 'Q' */

		static CoordRec[] char81_stroke0 = {
			new CoordRec(33.8714F, 100F),
			new CoordRec(24.3476F, 95.2381F),
			new CoordRec(14.8238F, 85.7143F),
			new CoordRec(10.0619F, 76.1905F),
			new CoordRec(5.3F, 61.9048F),
			new CoordRec(5.3F, 38.0952F),
			new CoordRec(10.0619F, 23.8095F),
			new CoordRec(14.8238F, 14.2857F),
			new CoordRec(24.3476F, 4.7619F),
			new CoordRec(33.8714F, 0F),
			new CoordRec(52.919F, 0F),
			new CoordRec(62.4429F, 4.7619F),
			new CoordRec(71.9667F, 14.2857F),
			new CoordRec(76.7286F, 23.8095F),
			new CoordRec(81.4905F, 38.0952F),
			new CoordRec(81.4905F, 61.9048F),
			new CoordRec(76.7286F, 76.1905F),
			new CoordRec(71.9667F, 85.7143F),
			new CoordRec(62.4429F, 95.2381F),
			new CoordRec(52.919F, 100F),
			new CoordRec(33.8714F, 100F)

		};
		static CoordRec[] char81_stroke1 = {
			new CoordRec(48.1571F, 19.0476F),
			new CoordRec(76.7286F, -9.5238F)

		};
		static StrokeRec[] char81 = {
			new StrokeRec(21, char81_stroke0),
			new StrokeRec(2, char81_stroke1)

		};
		// char: 82 'R' */

		static CoordRec[] char82_stroke0 = {
			new CoordRec(11.68F, 100F),
			new CoordRec(11.68F, 0F)

		};
		static CoordRec[] char82_stroke1 = {
			new CoordRec(11.68F, 100F),
			new CoordRec(54.5371F, 100F),
			new CoordRec(68.8229F, 95.2381F),
			new CoordRec(73.5848F, 90.4762F),
			new CoordRec(78.3467F, 80.9524F),
			new CoordRec(78.3467F, 71.4286F),
			new CoordRec(73.5848F, 61.9048F),
			new CoordRec(68.8229F, 57.1429F),
			new CoordRec(54.5371F, 52.381F),
			new CoordRec(11.68F, 52.381F)

		};
		static CoordRec[] char82_stroke2 = {
			new CoordRec(45.0133F, 52.381F),
			new CoordRec(78.3467F, 0F)

		};
		static StrokeRec[] char82 = {
			new StrokeRec(2, char82_stroke0),
			new StrokeRec(10, char82_stroke1),
			new StrokeRec(2, char82_stroke2)

		};
		// char: 83 'S' */

		static CoordRec[] char83_stroke0 = {
			new CoordRec(74.6667F, 85.7143F),
			new CoordRec(65.1429F, 95.2381F),
			new CoordRec(50.8571F, 100F),
			new CoordRec(31.8095F, 100F),
			new CoordRec(17.5238F, 95.2381F),
			new CoordRec(8F, 85.7143F),
			new CoordRec(8F, 76.1905F),
			new CoordRec(12.7619F, 66.6667F),
			new CoordRec(17.5238F, 61.9048F),
			new CoordRec(27.0476F, 57.1429F),
			new CoordRec(55.619F, 47.619F),
			new CoordRec(65.1429F, 42.8571F),
			new CoordRec(69.9048F, 38.0952F),
			new CoordRec(74.6667F, 28.5714F),
			new CoordRec(74.6667F, 14.2857F),
			new CoordRec(65.1429F, 4.7619F),
			new CoordRec(50.8571F, 0F),
			new CoordRec(31.8095F, 0F),
			new CoordRec(17.5238F, 4.7619F),
			new CoordRec(8F, 14.2857F)

		};

		static StrokeRec[] char83 = { new StrokeRec(20, char83_stroke0) };
		// char: 84 'T' */

		static CoordRec[] char84_stroke0 = {
			new CoordRec(35.6933F, 100F),
			new CoordRec(35.6933F, 0F)

		};
		static CoordRec[] char84_stroke1 = {
			new CoordRec(2.36F, 100F),
			new CoordRec(69.0267F, 100F)

		};
		static StrokeRec[] char84 = {
			new StrokeRec(2, char84_stroke0),
			new StrokeRec(2, char84_stroke1)

		};
		// char: 85 'U' */

		static CoordRec[] char85_stroke0 = {
			new CoordRec(11.54F, 100F),
			new CoordRec(11.54F, 28.5714F),
			new CoordRec(16.3019F, 14.2857F),
			new CoordRec(25.8257F, 4.7619F),
			new CoordRec(40.1114F, 0F),
			new CoordRec(49.6352F, 0F),
			new CoordRec(63.921F, 4.7619F),
			new CoordRec(73.4448F, 14.2857F),
			new CoordRec(78.2067F, 28.5714F),
			new CoordRec(78.2067F, 100F)

		};

		static StrokeRec[] char85 = { new StrokeRec(10, char85_stroke0) };
		// char: 86 'V' */

		static CoordRec[] char86_stroke0 = {
			new CoordRec(2.36F, 100F),
			new CoordRec(40.4552F, 0F)

		};
		static CoordRec[] char86_stroke1 = {
			new CoordRec(78.5505F, 100F),
			new CoordRec(40.4552F, 0F)

		};
		static StrokeRec[] char86 = {
			new StrokeRec(2, char86_stroke0),
			new StrokeRec(2, char86_stroke1)

		};
		// char: 87 'W' */

		static CoordRec[] char87_stroke0 = {
			new CoordRec(2.22F, 100F),
			new CoordRec(26.0295F, 0F)

		};
		static CoordRec[] char87_stroke1 = {
			new CoordRec(49.839F, 100F),
			new CoordRec(26.0295F, 0F)

		};
		static CoordRec[] char87_stroke2 = {
			new CoordRec(49.839F, 100F),
			new CoordRec(73.6486F, 0F)

		};
		static CoordRec[] char87_stroke3 = {
			new CoordRec(97.4581F, 100F),
			new CoordRec(73.6486F, 0F)

		};
		static StrokeRec[] char87 = {
			new StrokeRec(2, char87_stroke0),
			new StrokeRec(2, char87_stroke1),
			new StrokeRec(2, char87_stroke2),
			new StrokeRec(2, char87_stroke3)

		};
		// char: 88 'X' */

		static CoordRec[] char88_stroke0 = {
			new CoordRec(2.5F, 100F),
			new CoordRec(69.1667F, 0F)

		};
		static CoordRec[] char88_stroke1 = {
			new CoordRec(69.1667F, 100F),
			new CoordRec(2.5F, 0F)

		};
		static StrokeRec[] char88 = {
			new StrokeRec(2, char88_stroke0),
			new StrokeRec(2, char88_stroke1)

		};
		// char: 89 'Y' */

		static CoordRec[] char89_stroke0 = {
			new CoordRec(1.52F, 100F),
			new CoordRec(39.6152F, 52.381F),
			new CoordRec(39.6152F, 0F)

		};
		static CoordRec[] char89_stroke1 = {
			new CoordRec(77.7105F, 100F),
			new CoordRec(39.6152F, 52.381F)

		};
		static StrokeRec[] char89 = {
			new StrokeRec(3, char89_stroke0),
			new StrokeRec(2, char89_stroke1)

		};
		// char: 90 'Z' */

		static CoordRec[] char90_stroke0 = {
			new CoordRec(69.1667F, 100F),
			new CoordRec(2.5F, 0F)

		};
		static CoordRec[] char90_stroke1 = {
			new CoordRec(2.5F, 100F),
			new CoordRec(69.1667F, 100F)

		};
		static CoordRec[] char90_stroke2 = {
			new CoordRec(2.5F, 0F),
			new CoordRec(69.1667F, 0F)

		};
		static StrokeRec[] char90 = {
			new StrokeRec(2, char90_stroke0),
			new StrokeRec(2, char90_stroke1),
			new StrokeRec(2, char90_stroke2)

		};
		// char: 91 '[' */

		static CoordRec[] char91_stroke0 = {
			new CoordRec(7.78F, 119.048F),
			new CoordRec(7.78F, -33.3333F)

		};
		static CoordRec[] char91_stroke1 = {
			new CoordRec(12.5419F, 119.048F),
			new CoordRec(12.5419F, -33.3333F)

		};
		static CoordRec[] char91_stroke2 = {
			new CoordRec(7.78F, 119.048F),
			new CoordRec(41.1133F, 119.048F)

		};
		static CoordRec[] char91_stroke3 = {
			new CoordRec(7.78F, -33.3333F),
			new CoordRec(41.1133F, -33.3333F)

		};
		static StrokeRec[] char91 = {
			new StrokeRec(2, char91_stroke0),
			new StrokeRec(2, char91_stroke1),
			new StrokeRec(2, char91_stroke2),
			new StrokeRec(2, char91_stroke3)

		};
		// char: 92 '\' */

		static CoordRec[] char92_stroke0 = {
			new CoordRec(5.84F, 100F),
			new CoordRec(72.5067F, -14.2857F)

		};

		static StrokeRec[] char92 = { new StrokeRec(2, char92_stroke0) };
		// char: 93 ']' */

		static CoordRec[] char93_stroke0 = {
			new CoordRec(33.0114F, 119.048F),
			new CoordRec(33.0114F, -33.3333F)

		};
		static CoordRec[] char93_stroke1 = {
			new CoordRec(37.7733F, 119.048F),
			new CoordRec(37.7733F, -33.3333F)

		};
		static CoordRec[] char93_stroke2 = {
			new CoordRec(4.44F, 119.048F),
			new CoordRec(37.7733F, 119.048F)

		};
		static CoordRec[] char93_stroke3 = {
			new CoordRec(4.44F, -33.3333F),
			new CoordRec(37.7733F, -33.3333F)

		};
		static StrokeRec[] char93 = {
			new StrokeRec(2, char93_stroke0),
			new StrokeRec(2, char93_stroke1),
			new StrokeRec(2, char93_stroke2),
			new StrokeRec(2, char93_stroke3)

		};
		// char: 94 '^' */

		static CoordRec[] char94_stroke0 = {
			new CoordRec(44.0752F, 109.524F),
			new CoordRec(5.98F, 42.8571F)

		};
		static CoordRec[] char94_stroke1 = {
			new CoordRec(44.0752F, 109.524F),
			new CoordRec(82.1705F, 42.8571F)

		};
		static StrokeRec[] char94 = {
			new StrokeRec(2, char94_stroke0),
			new StrokeRec(2, char94_stroke1)

		};
		// char: 95 '_' */

		static CoordRec[] char95_stroke0 = {
			new CoordRec(-1.1F, -33.3333F),
			new CoordRec(103.662F, -33.3333F),
			new CoordRec(103.662F, -28.5714F),
			new CoordRec(-1.1F, -28.5714F),
			new CoordRec(-1.1F, -33.3333F)

		};

		static StrokeRec[] char95 = { new StrokeRec(5, char95_stroke0) };
		// char: 96 '`' */

		static CoordRec[] char96_stroke0 = {
			new CoordRec(33.0219F, 100F),
			new CoordRec(56.8314F, 71.4286F)

		};
		static CoordRec[] char96_stroke1 = {
			new CoordRec(33.0219F, 100F),
			new CoordRec(28.26F, 95.2381F),
			new CoordRec(56.8314F, 71.4286F)

		};
		static StrokeRec[] char96 = {
			new StrokeRec(2, char96_stroke0),
			new StrokeRec(3, char96_stroke1)

		};
		// char: 97 'a' */

		static CoordRec[] char97_stroke0 = {
			new CoordRec(63.8229F, 66.6667F),
			new CoordRec(63.8229F, 0F)

		};
		static CoordRec[] char97_stroke1 = {
			new CoordRec(63.8229F, 52.381F),
			new CoordRec(54.299F, 61.9048F),
			new CoordRec(44.7752F, 66.6667F),
			new CoordRec(30.4895F, 66.6667F),
			new CoordRec(20.9657F, 61.9048F),
			new CoordRec(11.4419F, 52.381F),
			new CoordRec(6.68F, 38.0952F),
			new CoordRec(6.68F, 28.5714F),
			new CoordRec(11.4419F, 14.2857F),
			new CoordRec(20.9657F, 4.7619F),
			new CoordRec(30.4895F, 0F),
			new CoordRec(44.7752F, 0F),
			new CoordRec(54.299F, 4.7619F),
			new CoordRec(63.8229F, 14.2857F)

		};
		static StrokeRec[] char97 = {
			new StrokeRec(2, char97_stroke0),
			new StrokeRec(14, char97_stroke1)

		};
		// char: 98 'b' */

		static CoordRec[] char98_stroke0 = {
			new CoordRec(8.76F, 100F),
			new CoordRec(8.76F, 0F)

		};
		static CoordRec[] char98_stroke1 = {
			new CoordRec(8.76F, 52.381F),
			new CoordRec(18.2838F, 61.9048F),
			new CoordRec(27.8076F, 66.6667F),
			new CoordRec(42.0933F, 66.6667F),
			new CoordRec(51.6171F, 61.9048F),
			new CoordRec(61.141F, 52.381F),
			new CoordRec(65.9029F, 38.0952F),
			new CoordRec(65.9029F, 28.5714F),
			new CoordRec(61.141F, 14.2857F),
			new CoordRec(51.6171F, 4.7619F),
			new CoordRec(42.0933F, 0F),
			new CoordRec(27.8076F, 0F),
			new CoordRec(18.2838F, 4.7619F),
			new CoordRec(8.76F, 14.2857F)

		};
		static StrokeRec[] char98 = {
			new StrokeRec(2, char98_stroke0),
			new StrokeRec(14, char98_stroke1)

		};
		// char: 99 'c' */

		static CoordRec[] char99_stroke0 = {
			new CoordRec(62.6629F, 52.381F),
			new CoordRec(53.139F, 61.9048F),
			new CoordRec(43.6152F, 66.6667F),
			new CoordRec(29.3295F, 66.6667F),
			new CoordRec(19.8057F, 61.9048F),
			new CoordRec(10.2819F, 52.381F),
			new CoordRec(5.52F, 38.0952F),
			new CoordRec(5.52F, 28.5714F),
			new CoordRec(10.2819F, 14.2857F),
			new CoordRec(19.8057F, 4.7619F),
			new CoordRec(29.3295F, 0F),
			new CoordRec(43.6152F, 0F),
			new CoordRec(53.139F, 4.7619F),
			new CoordRec(62.6629F, 14.2857F)

		};

		static StrokeRec[] char99 = { new StrokeRec(14, char99_stroke0) };
		// char: 100 'd' */

		static CoordRec[] char100_stroke0 = {
			new CoordRec(61.7829F, 100F),
			new CoordRec(61.7829F, 0F)

		};
		static CoordRec[] char100_stroke1 = {
			new CoordRec(61.7829F, 52.381F),
			new CoordRec(52.259F, 61.9048F),
			new CoordRec(42.7352F, 66.6667F),
			new CoordRec(28.4495F, 66.6667F),
			new CoordRec(18.9257F, 61.9048F),
			new CoordRec(9.4019F, 52.381F),
			new CoordRec(4.64F, 38.0952F),
			new CoordRec(4.64F, 28.5714F),
			new CoordRec(9.4019F, 14.2857F),
			new CoordRec(18.9257F, 4.7619F),
			new CoordRec(28.4495F, 0F),
			new CoordRec(42.7352F, 0F),
			new CoordRec(52.259F, 4.7619F),
			new CoordRec(61.7829F, 14.2857F)

		};
		static StrokeRec[] char100 = {
			new StrokeRec(2, char100_stroke0),
			new StrokeRec(14, char100_stroke1)

		};
		// char: 101 'e' */

		static CoordRec[] char101_stroke0 = {
			new CoordRec(5.72F, 38.0952F),
			new CoordRec(62.8629F, 38.0952F),
			new CoordRec(62.8629F, 47.619F),
			new CoordRec(58.101F, 57.1429F),
			new CoordRec(53.339F, 61.9048F),
			new CoordRec(43.8152F, 66.6667F),
			new CoordRec(29.5295F, 66.6667F),
			new CoordRec(20.0057F, 61.9048F),
			new CoordRec(10.4819F, 52.381F),
			new CoordRec(5.72F, 38.0952F),
			new CoordRec(5.72F, 28.5714F),
			new CoordRec(10.4819F, 14.2857F),
			new CoordRec(20.0057F, 4.7619F),
			new CoordRec(29.5295F, 0F),
			new CoordRec(43.8152F, 0F),
			new CoordRec(53.339F, 4.7619F),
			new CoordRec(62.8629F, 14.2857F)

		};

		static StrokeRec[] char101 = { new StrokeRec(17, char101_stroke0) };
		// char: 102 'f' */

		static CoordRec[] char102_stroke0 = {
			new CoordRec(38.7752F, 100F),
			new CoordRec(29.2514F, 100F),
			new CoordRec(19.7276F, 95.2381F),
			new CoordRec(14.9657F, 80.9524F),
			new CoordRec(14.9657F, 0F)

		};
		static CoordRec[] char102_stroke1 = {
			new CoordRec(0.68F, 66.6667F),
			new CoordRec(34.0133F, 66.6667F)

		};
		static StrokeRec[] char102 = {
			new StrokeRec(5, char102_stroke0),
			new StrokeRec(2, char102_stroke1)

		};
		// char: 103 'g' */

		static CoordRec[] char103_stroke0 = {
			new CoordRec(62.5029F, 66.6667F),
			new CoordRec(62.5029F, -9.5238F),
			new CoordRec(57.741F, -23.8095F),
			new CoordRec(52.979F, -28.5714F),
			new CoordRec(43.4552F, -33.3333F),
			new CoordRec(29.1695F, -33.3333F),
			new CoordRec(19.6457F, -28.5714F)

		};
		static CoordRec[] char103_stroke1 = {
			new CoordRec(62.5029F, 52.381F),
			new CoordRec(52.979F, 61.9048F),
			new CoordRec(43.4552F, 66.6667F),
			new CoordRec(29.1695F, 66.6667F),
			new CoordRec(19.6457F, 61.9048F),
			new CoordRec(10.1219F, 52.381F),
			new CoordRec(5.36F, 38.0952F),
			new CoordRec(5.36F, 28.5714F),
			new CoordRec(10.1219F, 14.2857F),
			new CoordRec(19.6457F, 4.7619F),
			new CoordRec(29.1695F, 0F),
			new CoordRec(43.4552F, 0F),
			new CoordRec(52.979F, 4.7619F),
			new CoordRec(62.5029F, 14.2857F)

		};
		static StrokeRec[] char103 = {
			new StrokeRec(7, char103_stroke0),
			new StrokeRec(14, char103_stroke1)

		};
		// char: 104 'h' */

		static CoordRec[] char104_stroke0 = {
			new CoordRec(9.6F, 100F),
			new CoordRec(9.6F, 0F)

		};
		static CoordRec[] char104_stroke1 = {
			new CoordRec(9.6F, 47.619F),
			new CoordRec(23.8857F, 61.9048F),
			new CoordRec(33.4095F, 66.6667F),
			new CoordRec(47.6952F, 66.6667F),
			new CoordRec(57.219F, 61.9048F),
			new CoordRec(61.981F, 47.619F),
			new CoordRec(61.981F, 0F)

		};
		static StrokeRec[] char104 = {
			new StrokeRec(2, char104_stroke0),
			new StrokeRec(7, char104_stroke1)

		};
		// char: 105 'i' */

		static CoordRec[] char105_stroke0 = {
			new CoordRec(10.02F, 100F),
			new CoordRec(14.7819F, 95.2381F),
			new CoordRec(19.5438F, 100F),
			new CoordRec(14.7819F, 104.762F),
			new CoordRec(10.02F, 100F)

		};
		static CoordRec[] char105_stroke1 = {
			new CoordRec(14.7819F, 66.6667F),
			new CoordRec(14.7819F, 0F)

		};
		static StrokeRec[] char105 = {
			new StrokeRec(5, char105_stroke0),
			new StrokeRec(2, char105_stroke1)

		};
		// char: 106 'j' */

		static CoordRec[] char106_stroke0 = {
			new CoordRec(17.3876F, 100F),
			new CoordRec(22.1495F, 95.2381F),
			new CoordRec(26.9114F, 100F),
			new CoordRec(22.1495F, 104.762F),
			new CoordRec(17.3876F, 100F)

		};
		static CoordRec[] char106_stroke1 = {
			new CoordRec(22.1495F, 66.6667F),
			new CoordRec(22.1495F, -14.2857F),
			new CoordRec(17.3876F, -28.5714F),
			new CoordRec(7.8638F, -33.3333F),
			new CoordRec(-1.66F, -33.3333F)

		};
		static StrokeRec[] char106 = {
			new StrokeRec(5, char106_stroke0),
			new StrokeRec(5, char106_stroke1)

		};
		// char: 107 'k' */

		static CoordRec[] char107_stroke0 = {
			new CoordRec(9.6F, 100F),
			new CoordRec(9.6F, 0F)

		};
		static CoordRec[] char107_stroke1 = {
			new CoordRec(57.219F, 66.6667F),
			new CoordRec(9.6F, 19.0476F)

		};
		static CoordRec[] char107_stroke2 = {
			new CoordRec(28.6476F, 38.0952F),
			new CoordRec(61.981F, 0F)

		};
		static StrokeRec[] char107 = {
			new StrokeRec(2, char107_stroke0),
			new StrokeRec(2, char107_stroke1),
			new StrokeRec(2, char107_stroke2)

		};
		// char: 108 'l' */

		static CoordRec[] char108_stroke0 = {
			new CoordRec(10.02F, 100F),
			new CoordRec(10.02F, 0F)

		};

		static StrokeRec[] char108 = { new StrokeRec(2, char108_stroke0) };
		// char: 109 'm' */

		static CoordRec[] char109_stroke0 = {
			new CoordRec(9.6F, 66.6667F),
			new CoordRec(9.6F, 0F)

		};
		static CoordRec[] char109_stroke1 = {
			new CoordRec(9.6F, 47.619F),
			new CoordRec(23.8857F, 61.9048F),
			new CoordRec(33.4095F, 66.6667F),
			new CoordRec(47.6952F, 66.6667F),
			new CoordRec(57.219F, 61.9048F),
			new CoordRec(61.981F, 47.619F),
			new CoordRec(61.981F, 0F)

		};
		static CoordRec[] char109_stroke2 = {
			new CoordRec(61.981F, 47.619F),
			new CoordRec(76.2667F, 61.9048F),
			new CoordRec(85.7905F, 66.6667F),
			new CoordRec(100.076F, 66.6667F),
			new CoordRec(109.6F, 61.9048F),
			new CoordRec(114.362F, 47.619F),
			new CoordRec(114.362F, 0F)

		};
		static StrokeRec[] char109 = {
			new StrokeRec(2, char109_stroke0),
			new StrokeRec(7, char109_stroke1),
			new StrokeRec(7, char109_stroke2)

		};
		// char: 110 'n' */

		static CoordRec[] char110_stroke0 = {
			new CoordRec(9.18F, 66.6667F),
			new CoordRec(9.18F, 0F)

		};
		static CoordRec[] char110_stroke1 = {
			new CoordRec(9.18F, 47.619F),
			new CoordRec(23.4657F, 61.9048F),
			new CoordRec(32.9895F, 66.6667F),
			new CoordRec(47.2752F, 66.6667F),
			new CoordRec(56.799F, 61.9048F),
			new CoordRec(61.561F, 47.619F),
			new CoordRec(61.561F, 0F)

		};
		static StrokeRec[] char110 = {
			new StrokeRec(2, char110_stroke0),
			new StrokeRec(7, char110_stroke1)

		};
		// char: 111 'o' */

		static CoordRec[] char111_stroke0 = {
			new CoordRec(28.7895F, 66.6667F),
			new CoordRec(19.2657F, 61.9048F),
			new CoordRec(9.7419F, 52.381F),
			new CoordRec(4.98F, 38.0952F),
			new CoordRec(4.98F, 28.5714F),
			new CoordRec(9.7419F, 14.2857F),
			new CoordRec(19.2657F, 4.7619F),
			new CoordRec(28.7895F, 0F),
			new CoordRec(43.0752F, 0F),
			new CoordRec(52.599F, 4.7619F),
			new CoordRec(62.1229F, 14.2857F),
			new CoordRec(66.8848F, 28.5714F),
			new CoordRec(66.8848F, 38.0952F),
			new CoordRec(62.1229F, 52.381F),
			new CoordRec(52.599F, 61.9048F),
			new CoordRec(43.0752F, 66.6667F),
			new CoordRec(28.7895F, 66.6667F)

		};

		static StrokeRec[] char111 = { new StrokeRec(17, char111_stroke0) };
		// char: 112 'p' */

		static CoordRec[] char112_stroke0 = {
			new CoordRec(9.46F, 66.6667F),
			new CoordRec(9.46F, -33.3333F)

		};
		static CoordRec[] char112_stroke1 = {
			new CoordRec(9.46F, 52.381F),
			new CoordRec(18.9838F, 61.9048F),
			new CoordRec(28.5076F, 66.6667F),
			new CoordRec(42.7933F, 66.6667F),
			new CoordRec(52.3171F, 61.9048F),
			new CoordRec(61.841F, 52.381F),
			new CoordRec(66.6029F, 38.0952F),
			new CoordRec(66.6029F, 28.5714F),
			new CoordRec(61.841F, 14.2857F),
			new CoordRec(52.3171F, 4.7619F),
			new CoordRec(42.7933F, 0F),
			new CoordRec(28.5076F, 0F),
			new CoordRec(18.9838F, 4.7619F),
			new CoordRec(9.46F, 14.2857F)

		};
		static StrokeRec[] char112 = {
			new StrokeRec(2, char112_stroke0),
			new StrokeRec(14, char112_stroke1)

		};
		// char: 113 'q' */

		static CoordRec[] char113_stroke0 = {
			new CoordRec(61.9829F, 66.6667F),
			new CoordRec(61.9829F, -33.3333F)

		};
		static CoordRec[] char113_stroke1 = {
			new CoordRec(61.9829F, 52.381F),
			new CoordRec(52.459F, 61.9048F),
			new CoordRec(42.9352F, 66.6667F),
			new CoordRec(28.6495F, 66.6667F),
			new CoordRec(19.1257F, 61.9048F),
			new CoordRec(9.6019F, 52.381F),
			new CoordRec(4.84F, 38.0952F),
			new CoordRec(4.84F, 28.5714F),
			new CoordRec(9.6019F, 14.2857F),
			new CoordRec(19.1257F, 4.7619F),
			new CoordRec(28.6495F, 0F),
			new CoordRec(42.9352F, 0F),
			new CoordRec(52.459F, 4.7619F),
			new CoordRec(61.9829F, 14.2857F)

		};
		static StrokeRec[] char113 = {
			new StrokeRec(2, char113_stroke0),
			new StrokeRec(14, char113_stroke1)

		};
		// char: 114 'r' */

		static CoordRec[] char114_stroke0 = {
			new CoordRec(9.46F, 66.6667F),
			new CoordRec(9.46F, 0F)

		};
		static CoordRec[] char114_stroke1 = {
			new CoordRec(9.46F, 38.0952F),
			new CoordRec(14.2219F, 52.381F),
			new CoordRec(23.7457F, 61.9048F),
			new CoordRec(33.2695F, 66.6667F),
			new CoordRec(47.5552F, 66.6667F)

		};
		static StrokeRec[] char114 = {
			new StrokeRec(2, char114_stroke0),
			new StrokeRec(5, char114_stroke1)

		};
		// char: 115 's' */

		static CoordRec[] char115_stroke0 = {
			new CoordRec(57.081F, 52.381F),
			new CoordRec(52.319F, 61.9048F),
			new CoordRec(38.0333F, 66.6667F),
			new CoordRec(23.7476F, 66.6667F),
			new CoordRec(9.4619F, 61.9048F),
			new CoordRec(4.7F, 52.381F),
			new CoordRec(9.4619F, 42.8571F),
			new CoordRec(18.9857F, 38.0952F),
			new CoordRec(42.7952F, 33.3333F),
			new CoordRec(52.319F, 28.5714F),
			new CoordRec(57.081F, 19.0476F),
			new CoordRec(57.081F, 14.2857F),
			new CoordRec(52.319F, 4.7619F),
			new CoordRec(38.0333F, 0F),
			new CoordRec(23.7476F, 0F),
			new CoordRec(9.4619F, 4.7619F),
			new CoordRec(4.7F, 14.2857F)

		};

		static StrokeRec[] char115 = { new StrokeRec(17, char115_stroke0) };
		// char: 116 't' */

		static CoordRec[] char116_stroke0 = {
			new CoordRec(14.8257F, 100F),
			new CoordRec(14.8257F, 19.0476F),
			new CoordRec(19.5876F, 4.7619F),
			new CoordRec(29.1114F, 0F),
			new CoordRec(38.6352F, 0F)

		};
		static CoordRec[] char116_stroke1 = {
			new CoordRec(0.54F, 66.6667F),
			new CoordRec(33.8733F, 66.6667F)

		};
		static StrokeRec[] char116 = {
			new StrokeRec(5, char116_stroke0),
			new StrokeRec(2, char116_stroke1)

		};
		// char: 117 'u' */

		static CoordRec[] char117_stroke0 = {
			new CoordRec(9.46F, 66.6667F),
			new CoordRec(9.46F, 19.0476F),
			new CoordRec(14.2219F, 4.7619F),
			new CoordRec(23.7457F, 0F),
			new CoordRec(38.0314F, 0F),
			new CoordRec(47.5552F, 4.7619F),
			new CoordRec(61.841F, 19.0476F)

		};
		static CoordRec[] char117_stroke1 = {
			new CoordRec(61.841F, 66.6667F),
			new CoordRec(61.841F, 0F)

		};
		static StrokeRec[] char117 = {
			new StrokeRec(7, char117_stroke0),
			new StrokeRec(2, char117_stroke1)

		};
		// char: 118 'v' */

		static CoordRec[] char118_stroke0 = {
			new CoordRec(1.8F, 66.6667F),
			new CoordRec(30.3714F, 0F)

		};
		static CoordRec[] char118_stroke1 = {
			new CoordRec(58.9429F, 66.6667F),
			new CoordRec(30.3714F, 0F)

		};
		static StrokeRec[] char118 = {
			new StrokeRec(2, char118_stroke0),
			new StrokeRec(2, char118_stroke1)

		};
		// char: 119 'w' */

		static CoordRec[] char119_stroke0 = {
			new CoordRec(2.5F, 66.6667F),
			new CoordRec(21.5476F, 0F)

		};
		static CoordRec[] char119_stroke1 = {
			new CoordRec(40.5952F, 66.6667F),
			new CoordRec(21.5476F, 0F)

		};
		static CoordRec[] char119_stroke2 = {
			new CoordRec(40.5952F, 66.6667F),
			new CoordRec(59.6429F, 0F)

		};
		static CoordRec[] char119_stroke3 = {
			new CoordRec(78.6905F, 66.6667F),
			new CoordRec(59.6429F, 0F)

		};
		static StrokeRec[] char119 = {
			new StrokeRec(2, char119_stroke0),
			new StrokeRec(2, char119_stroke1),
			new StrokeRec(2, char119_stroke2),
			new StrokeRec(2, char119_stroke3)

		};
		// char: 120 'x' */

		static CoordRec[] char120_stroke0 = {
			new CoordRec(1.66F, 66.6667F),
			new CoordRec(54.041F, 0F)

		};
		static CoordRec[] char120_stroke1 = {
			new CoordRec(54.041F, 66.6667F),
			new CoordRec(1.66F, 0F)

		};
		static StrokeRec[] char120 = {
			new StrokeRec(2, char120_stroke0),
			new StrokeRec(2, char120_stroke1)

		};
		// char: 121 'y' */

		static CoordRec[] char121_stroke0 = {
			new CoordRec(6.5619F, 66.6667F),
			new CoordRec(35.1333F, 0F)

		};
		static CoordRec[] char121_stroke1 = {
			new CoordRec(63.7048F, 66.6667F),
			new CoordRec(35.1333F, 0F),
			new CoordRec(25.6095F, -19.0476F),
			new CoordRec(16.0857F, -28.5714F),
			new CoordRec(6.5619F, -33.3333F),
			new CoordRec(1.8F, -33.3333F)

		};
		static StrokeRec[] char121 = {
			new StrokeRec(2, char121_stroke0),
			new StrokeRec(6, char121_stroke1)

		};
		// char: 122 'z' */

		static CoordRec[] char122_stroke0 = {
			new CoordRec(56.821F, 66.6667F),
			new CoordRec(4.44F, 0F)

		};
		static CoordRec[] char122_stroke1 = {
			new CoordRec(4.44F, 66.6667F),
			new CoordRec(56.821F, 66.6667F)

		};
		static CoordRec[] char122_stroke2 = {
			new CoordRec(4.44F, 0F),
			new CoordRec(56.821F, 0F)

		};
		static StrokeRec[] char122 = {
			new StrokeRec(2, char122_stroke0),
			new StrokeRec(2, char122_stroke1),
			new StrokeRec(2, char122_stroke2)

		};
		// char: 123 '{' */

		static CoordRec[] char123_stroke0 = {
			new CoordRec(31.1895F, 119.048F),
			new CoordRec(21.6657F, 114.286F),
			new CoordRec(16.9038F, 109.524F),
			new CoordRec(12.1419F, 100F),
			new CoordRec(12.1419F, 90.4762F),
			new CoordRec(16.9038F, 80.9524F),
			new CoordRec(21.6657F, 76.1905F),
			new CoordRec(26.4276F, 66.6667F),
			new CoordRec(26.4276F, 57.1429F),
			new CoordRec(16.9038F, 47.619F)

		};
		static CoordRec[] char123_stroke1 = {
			new CoordRec(21.6657F, 114.286F),
			new CoordRec(16.9038F, 104.762F),
			new CoordRec(16.9038F, 95.2381F),
			new CoordRec(21.6657F, 85.7143F),
			new CoordRec(26.4276F, 80.9524F),
			new CoordRec(31.1895F, 71.4286F),
			new CoordRec(31.1895F, 61.9048F),
			new CoordRec(26.4276F, 52.381F),
			new CoordRec(7.38F, 42.8571F),
			new CoordRec(26.4276F, 33.3333F),
			new CoordRec(31.1895F, 23.8095F),
			new CoordRec(31.1895F, 14.2857F),
			new CoordRec(26.4276F, 4.7619F),
			new CoordRec(21.6657F, 0F),
			new CoordRec(16.9038F, -9.5238F),
			new CoordRec(16.9038F, -19.0476F),
			new CoordRec(21.6657F, -28.5714F)

		};
		static CoordRec[] char123_stroke2 = {
			new CoordRec(16.9038F, 38.0952F),
			new CoordRec(26.4276F, 28.5714F),
			new CoordRec(26.4276F, 19.0476F),
			new CoordRec(21.6657F, 9.5238F),
			new CoordRec(16.9038F, 4.7619F),
			new CoordRec(12.1419F, -4.7619F),
			new CoordRec(12.1419F, -14.2857F),
			new CoordRec(16.9038F, -23.8095F),
			new CoordRec(21.6657F, -28.5714F),
			new CoordRec(31.1895F, -33.3333F)

		};
		static StrokeRec[] char123 = {
			new StrokeRec(10, char123_stroke0),
			new StrokeRec(17, char123_stroke1),
			new StrokeRec(10, char123_stroke2)

		};
		// char: 124 '|' */

		static CoordRec[] char124_stroke0 = {
			new CoordRec(11.54F, 119.048F),
			new CoordRec(11.54F, -33.3333F)

		};

		static StrokeRec[] char124 = { new StrokeRec(2, char124_stroke0) };
		// char: 125 '}' */

		static CoordRec[] char125_stroke0 = {
			new CoordRec(9.18F, 119.048F),
			new CoordRec(18.7038F, 114.286F),
			new CoordRec(23.4657F, 109.524F),
			new CoordRec(28.2276F, 100F),
			new CoordRec(28.2276F, 90.4762F),
			new CoordRec(23.4657F, 80.9524F),
			new CoordRec(18.7038F, 76.1905F),
			new CoordRec(13.9419F, 66.6667F),
			new CoordRec(13.9419F, 57.1429F),
			new CoordRec(23.4657F, 47.619F)

		};
		static CoordRec[] char125_stroke1 = {
			new CoordRec(18.7038F, 114.286F),
			new CoordRec(23.4657F, 104.762F),
			new CoordRec(23.4657F, 95.2381F),
			new CoordRec(18.7038F, 85.7143F),
			new CoordRec(13.9419F, 80.9524F),
			new CoordRec(9.18F, 71.4286F),
			new CoordRec(9.18F, 61.9048F),
			new CoordRec(13.9419F, 52.381F),
			new CoordRec(32.9895F, 42.8571F),
			new CoordRec(13.9419F, 33.3333F),
			new CoordRec(9.18F, 23.8095F),
			new CoordRec(9.18F, 14.2857F),
			new CoordRec(13.9419F, 4.7619F),
			new CoordRec(18.7038F, 0F),
			new CoordRec(23.4657F, -9.5238F),
			new CoordRec(23.4657F, -19.0476F),
			new CoordRec(18.7038F, -28.5714F)

		};
		static CoordRec[] char125_stroke2 = {
			new CoordRec(23.4657F, 38.0952F),
			new CoordRec(13.9419F, 28.5714F),
			new CoordRec(13.9419F, 19.0476F),
			new CoordRec(18.7038F, 9.5238F),
			new CoordRec(23.4657F, 4.7619F),
			new CoordRec(28.2276F, -4.7619F),
			new CoordRec(28.2276F, -14.2857F),
			new CoordRec(23.4657F, -23.8095F),
			new CoordRec(18.7038F, -28.5714F),
			new CoordRec(9.18F, -33.3333F)

		};
		static StrokeRec[] char125 = {
			new StrokeRec(10, char125_stroke0),
			new StrokeRec(17, char125_stroke1),
			new StrokeRec(10, char125_stroke2)

		};
		// char: 126 '~' */

		static CoordRec[] char126_stroke0 = {
			new CoordRec(2.92F, 28.5714F),
			new CoordRec(2.92F, 38.0952F),
			new CoordRec(7.6819F, 52.381F),
			new CoordRec(17.2057F, 57.1429F),
			new CoordRec(26.7295F, 57.1429F),
			new CoordRec(36.2533F, 52.381F),
			new CoordRec(55.301F, 38.0952F),
			new CoordRec(64.8248F, 33.3333F),
			new CoordRec(74.3486F, 33.3333F),
			new CoordRec(83.8724F, 38.0952F),
			new CoordRec(88.6343F, 47.619F)

		};
		static CoordRec[] char126_stroke1 = {
			new CoordRec(2.92F, 38.0952F),
			new CoordRec(7.6819F, 47.619F),
			new CoordRec(17.2057F, 52.381F),
			new CoordRec(26.7295F, 52.381F),
			new CoordRec(36.2533F, 47.619F),
			new CoordRec(55.301F, 33.3333F),
			new CoordRec(64.8248F, 28.5714F),
			new CoordRec(74.3486F, 28.5714F),
			new CoordRec(83.8724F, 33.3333F),
			new CoordRec(88.6343F, 47.619F),
			new CoordRec(88.6343F, 57.1429F)

		};
		static StrokeRec[] char126 = {
			new StrokeRec(11, char126_stroke0),
			new StrokeRec(11, char126_stroke1)

		};
		// char: 127 */

		static CoordRec[] char127_stroke0 = {
			new CoordRec(52.381F, 100F),
			new CoordRec(14.2857F, -33.3333F)

		};
		static CoordRec[] char127_stroke1 = {
			new CoordRec(28.5714F, 66.6667F),
			new CoordRec(14.2857F, 61.9048F),
			new CoordRec(4.7619F, 52.381F),
			new CoordRec(0F, 38.0952F),
			new CoordRec(0F, 23.8095F),
			new CoordRec(4.7619F, 14.2857F),
			new CoordRec(14.2857F, 4.7619F),
			new CoordRec(28.5714F, 0F),
			new CoordRec(38.0952F, 0F),
			new CoordRec(52.381F, 4.7619F),
			new CoordRec(61.9048F, 14.2857F),
			new CoordRec(66.6667F, 28.5714F),
			new CoordRec(66.6667F, 42.8571F),
			new CoordRec(61.9048F, 52.381F),
			new CoordRec(52.381F, 61.9048F),
			new CoordRec(38.0952F, 66.6667F),
			new CoordRec(28.5714F, 66.6667F)

		};
		static StrokeRec[] char127 = {
			new StrokeRec(2, char127_stroke0),
			new StrokeRec(17, char127_stroke1)

		};
		static StrokeCharRec[] chars = {
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 0, 0),
			new StrokeCharRec(0, null, 52.381f, 104.762f),
			new StrokeCharRec(2, char33, 13.3819f, 26.6238f),
			new StrokeCharRec(2, char34, 23.0676f, 51.4352f),
			new StrokeCharRec(4, char35, 36.5333f, 79.4886f),
			new StrokeCharRec(3, char36, 38.1533f, 76.2067f),
			new StrokeCharRec(3, char37, 49.2171f, 96.5743f),
			new StrokeCharRec(1, char38, 53.599f, 101.758f),
			new StrokeCharRec(1, char39, 4.44f, 13.62f),
			new StrokeCharRec(1, char40, 21.8657f, 47.1733f),
			new StrokeCharRec(1, char41, 24.3276f, 47.5333f),
			new StrokeCharRec(3, char42, 30.7695f, 59.439f),
			new StrokeCharRec(2, char43, 48.8371f, 97.2543f),
			new StrokeCharRec(1, char44, 13.5219f, 26.0638f),
			new StrokeCharRec(1, char45, 50.2371f, 100.754f),
			new StrokeCharRec(1, char46, 13.1019f, 26.4838f),
			new StrokeCharRec(1, char47, 40.5733f, 82.1067f),
			new StrokeCharRec(1, char48, 38.3133f, 77.0667f),
			new StrokeCharRec(1, char49, 30.8676f, 66.5295f),
			new StrokeCharRec(1, char50, 38.7533f, 77.6467f),
			new StrokeCharRec(1, char51, 38.3333f, 77.0467f),
			new StrokeCharRec(2, char52, 37.2133f, 80.1686f),
			new StrokeCharRec(1, char53, 38.1933f, 77.6867f),
			new StrokeCharRec(1, char54, 34.1514f, 73.8048f),
			new StrokeCharRec(2, char55, 38.8933f, 77.2267f),
			new StrokeCharRec(1, char56, 38.9333f, 77.6667f),
			new StrokeCharRec(1, char57, 39.9333f, 74.0648f),
			new StrokeCharRec(2, char58, 14.0819f, 26.2238f),
			new StrokeCharRec(2, char59, 12.9619f, 26.3038f),
			new StrokeCharRec(1, char60, 41.1552f, 81.6105f),
			new StrokeCharRec(2, char61, 48.5571f, 97.2543f),
			new StrokeCharRec(1, char62, 40.8752f, 81.6105f),
			new StrokeCharRec(2, char63, 36.9914f, 73.9029f),
			new StrokeCharRec(2, char64, 34.9314f, 74.3648f),
			new StrokeCharRec(3, char65, 40.5952f, 80.4905f),
			new StrokeCharRec(3, char66, 44.7533f, 83.6267f),
			new StrokeCharRec(1, char67, 39.9933f, 84.4886f),
			new StrokeCharRec(2, char68, 45.2933f, 85.2867f),
			new StrokeCharRec(4, char69, 39.9914f, 78.1848f),
			new StrokeCharRec(3, char70, 39.9914f, 78.7448f),
			new StrokeCharRec(2, char71, 40.3933f, 89.7686f),
			new StrokeCharRec(3, char72, 44.7533f, 89.0867f),
			new StrokeCharRec(1, char73, 10.86f, 21.3f),
			new StrokeCharRec(1, char74, 31.0714f, 59.999f),
			new StrokeCharRec(3, char75, 44.6133f, 79.3267f),
			new StrokeCharRec(2, char76, 40.2514f, 71.3229f),
			new StrokeCharRec(4, char77, 48.9552f, 97.2105f),
			new StrokeCharRec(3, char78, 44.4733f, 88.8067f),
			new StrokeCharRec(1, char79, 44.3352f, 88.8305f),
			new StrokeCharRec(2, char80, 45.4333f, 85.6667f),
			new StrokeCharRec(2, char81, 43.3952f, 88.0905f),
			new StrokeCharRec(3, char82, 45.0133f, 82.3667f),
			new StrokeCharRec(1, char83, 41.3333f, 80.8267f),
			new StrokeCharRec(2, char84, 35.6933f, 71.9467f),
			new StrokeCharRec(1, char85, 44.8733f, 89.4867f),
			new StrokeCharRec(2, char86, 40.4552f, 81.6105f),
			new StrokeCharRec(4, char87, 49.839f, 100.518f),
			new StrokeCharRec(2, char88, 35.8333f, 72.3667f),
			new StrokeCharRec(2, char89, 39.6152f, 79.6505f),
			new StrokeCharRec(3, char90, 35.8333f, 73.7467f),
			new StrokeCharRec(4, char91, 22.0657f, 46.1133f),
			new StrokeCharRec(1, char92, 39.1733f, 78.2067f),
			new StrokeCharRec(4, char93, 23.4876f, 46.3933f),
			new StrokeCharRec(2, char94, 44.0752f, 90.2305f),
			new StrokeCharRec(1, char95, 51.281f, 104.062f),
			new StrokeCharRec(2, char96, 42.5457f, 83.5714f),
			new StrokeCharRec(2, char97, 35.2514f, 66.6029f),
			new StrokeCharRec(2, char98, 37.3314f, 70.4629f),
			new StrokeCharRec(1, char99, 34.0914f, 68.9229f),
			new StrokeCharRec(2, char100, 33.2114f, 70.2629f),
			new StrokeCharRec(1, char101, 34.2914f, 68.5229f),
			new StrokeCharRec(2, char102, 14.9657f, 38.6552f),
			new StrokeCharRec(2, char103, 33.9314f, 70.9829f),
			new StrokeCharRec(2, char104, 33.4095f, 71.021f),
			new StrokeCharRec(2, char105, 14.7819f, 28.8638f),
			new StrokeCharRec(2, char106, 17.3876f, 36.2314f),
			new StrokeCharRec(3, char107, 33.4095f, 62.521f),
			new StrokeCharRec(1, char108, 10.02f, 19.34f),
			new StrokeCharRec(3, char109, 61.981f, 123.962f),
			new StrokeCharRec(2, char110, 32.9895f, 70.881f),
			new StrokeCharRec(1, char111, 33.5514f, 71.7448f),
			new StrokeCharRec(2, char112, 38.0314f, 70.8029f),
			new StrokeCharRec(2, char113, 33.4114f, 70.7429f),
			new StrokeCharRec(2, char114, 23.7457f, 49.4952f),
			new StrokeCharRec(1, char115, 28.5095f, 62.321f),
			new StrokeCharRec(2, char116, 14.8257f, 39.3152f),
			new StrokeCharRec(2, char117, 33.2695f, 71.161f),
			new StrokeCharRec(2, char118, 30.3714f, 60.6029f),
			new StrokeCharRec(4, char119, 40.5952f, 80.4905f),
			new StrokeCharRec(2, char120, 25.4695f, 56.401f),
			new StrokeCharRec(2, char121, 35.1333f, 66.0648f),
			new StrokeCharRec(3, char122, 28.2495f, 61.821f),
			new StrokeCharRec(3, char123, 21.6657f, 41.6295f),
			new StrokeCharRec(1, char124, 11.54f, 23.78f),
			new StrokeCharRec(3, char125, 18.7038f, 41.4695f),
			new StrokeCharRec(2, char126, 45.7771f, 91.2743f),
			new StrokeCharRec(2, char127, 33.3333f, 66.6667f)

		};

		public static StrokeFontRec glutStrokeRoman = new StrokeFontRec("Roman", 128, chars, 119.048f, -33.3333f);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
