
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Glut
{

	public class GLUTStrokeMonoRoman
	{

		// char: 33 '!' */

		static CoordRec[] char33_stroke0 = {
			new CoordRec(52.381F, 100F),
			new CoordRec(52.381F, 33.3333F)

		};
		static CoordRec[] char33_stroke1 = {
			new CoordRec(52.381F, 9.5238F),
			new CoordRec(47.6191F, 4.7619F),
			new CoordRec(52.381F, 0F),
			new CoordRec(57.1429F, 4.7619F),
			new CoordRec(52.381F, 9.5238F)

		};
		static StrokeRec[] char33 = {
			new StrokeRec(2, char33_stroke0),
			new StrokeRec(5, char33_stroke1)

		};
		// char: 34 '"' */

		static CoordRec[] char34_stroke0 = {
			new CoordRec(33.3334F, 100F),
			new CoordRec(33.3334F, 66.6667F)

		};
		static CoordRec[] char34_stroke1 = {
			new CoordRec(71.4286F, 100F),
			new CoordRec(71.4286F, 66.6667F)

		};
		static StrokeRec[] char34 = {
			new StrokeRec(2, char34_stroke0),
			new StrokeRec(2, char34_stroke1)

		};
		// char: 35 '#' */

		static CoordRec[] char35_stroke0 = {
			new CoordRec(54.7619F, 119.048F),
			new CoordRec(21.4286F, -33.3333F)

		};
		static CoordRec[] char35_stroke1 = {
			new CoordRec(83.3334F, 119.048F),
			new CoordRec(50F, -33.3333F)

		};
		static CoordRec[] char35_stroke2 = {
			new CoordRec(21.4286F, 57.1429F),
			new CoordRec(88.0952F, 57.1429F)

		};
		static CoordRec[] char35_stroke3 = {
			new CoordRec(16.6667F, 28.5714F),
			new CoordRec(83.3334F, 28.5714F)

		};
		static StrokeRec[] char35 = {
			new StrokeRec(2, char35_stroke0),
			new StrokeRec(2, char35_stroke1),
			new StrokeRec(2, char35_stroke2),
			new StrokeRec(2, char35_stroke3)

		};
		// char: 36 '$' */

		static CoordRec[] char36_stroke0 = {
			new CoordRec(42.8571F, 119.048F),
			new CoordRec(42.8571F, -19.0476F)

		};
		static CoordRec[] char36_stroke1 = {
			new CoordRec(61.9047F, 119.048F),
			new CoordRec(61.9047F, -19.0476F)

		};
		static CoordRec[] char36_stroke2 = {
			new CoordRec(85.7143F, 85.7143F),
			new CoordRec(76.1905F, 95.2381F),
			new CoordRec(61.9047F, 100F),
			new CoordRec(42.8571F, 100F),
			new CoordRec(28.5714F, 95.2381F),
			new CoordRec(19.0476F, 85.7143F),
			new CoordRec(19.0476F, 76.1905F),
			new CoordRec(23.8095F, 66.6667F),
			new CoordRec(28.5714F, 61.9048F),
			new CoordRec(38.0952F, 57.1429F),
			new CoordRec(66.6666F, 47.619F),
			new CoordRec(76.1905F, 42.8571F),
			new CoordRec(80.9524F, 38.0952F),
			new CoordRec(85.7143F, 28.5714F),
			new CoordRec(85.7143F, 14.2857F),
			new CoordRec(76.1905F, 4.7619F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(42.8571F, 0F),
			new CoordRec(28.5714F, 4.7619F),
			new CoordRec(19.0476F, 14.2857F)

		};
		static StrokeRec[] char36 = {
			new StrokeRec(2, char36_stroke0),
			new StrokeRec(2, char36_stroke1),
			new StrokeRec(20, char36_stroke2)

		};
		// char: 37 '%' */

		static CoordRec[] char37_stroke0 = {
			new CoordRec(95.2381F, 100F),
			new CoordRec(9.5238F, 0F)

		};
		static CoordRec[] char37_stroke1 = {
			new CoordRec(33.3333F, 100F),
			new CoordRec(42.8571F, 90.4762F),
			new CoordRec(42.8571F, 80.9524F),
			new CoordRec(38.0952F, 71.4286F),
			new CoordRec(28.5714F, 66.6667F),
			new CoordRec(19.0476F, 66.6667F),
			new CoordRec(9.5238F, 76.1905F),
			new CoordRec(9.5238F, 85.7143F),
			new CoordRec(14.2857F, 95.2381F),
			new CoordRec(23.8095F, 100F),
			new CoordRec(33.3333F, 100F),
			new CoordRec(42.8571F, 95.2381F),
			new CoordRec(57.1428F, 90.4762F),
			new CoordRec(71.4286F, 90.4762F),
			new CoordRec(85.7143F, 95.2381F),
			new CoordRec(95.2381F, 100F)

		};
		static CoordRec[] char37_stroke2 = {
			new CoordRec(76.1905F, 33.3333F),
			new CoordRec(66.6667F, 28.5714F),
			new CoordRec(61.9048F, 19.0476F),
			new CoordRec(61.9048F, 9.5238F),
			new CoordRec(71.4286F, 0F),
			new CoordRec(80.9524F, 0F),
			new CoordRec(90.4762F, 4.7619F),
			new CoordRec(95.2381F, 14.2857F),
			new CoordRec(95.2381F, 23.8095F),
			new CoordRec(85.7143F, 33.3333F),
			new CoordRec(76.1905F, 33.3333F)

		};
		static StrokeRec[] char37 = {
			new StrokeRec(2, char37_stroke0),
			new StrokeRec(16, char37_stroke1),
			new StrokeRec(11, char37_stroke2)

		};
		// char: 38 '&' */

		static CoordRec[] char38_stroke0 = {
			new CoordRec(100F, 57.1429F),
			new CoordRec(100F, 61.9048F),
			new CoordRec(95.2381F, 66.6667F),
			new CoordRec(90.4762F, 66.6667F),
			new CoordRec(85.7143F, 61.9048F),
			new CoordRec(80.9524F, 52.381F),
			new CoordRec(71.4286F, 28.5714F),
			new CoordRec(61.9048F, 14.2857F),
			new CoordRec(52.3809F, 4.7619F),
			new CoordRec(42.8571F, 0F),
			new CoordRec(23.8095F, 0F),
			new CoordRec(14.2857F, 4.7619F),
			new CoordRec(9.5238F, 9.5238F),
			new CoordRec(4.7619F, 19.0476F),
			new CoordRec(4.7619F, 28.5714F),
			new CoordRec(9.5238F, 38.0952F),
			new CoordRec(14.2857F, 42.8571F),
			new CoordRec(47.619F, 61.9048F),
			new CoordRec(52.3809F, 66.6667F),
			new CoordRec(57.1429F, 76.1905F),
			new CoordRec(57.1429F, 85.7143F),
			new CoordRec(52.3809F, 95.2381F),
			new CoordRec(42.8571F, 100F),
			new CoordRec(33.3333F, 95.2381F),
			new CoordRec(28.5714F, 85.7143F),
			new CoordRec(28.5714F, 76.1905F),
			new CoordRec(33.3333F, 61.9048F),
			new CoordRec(42.8571F, 47.619F),
			new CoordRec(66.6667F, 14.2857F),
			new CoordRec(76.1905F, 4.7619F),
			new CoordRec(85.7143F, 0F),
			new CoordRec(95.2381F, 0F),
			new CoordRec(100F, 4.7619F),
			new CoordRec(100F, 9.5238F)

		};

		static StrokeRec[] char38 = { new StrokeRec(34, char38_stroke0) };
		// char: 39 ''' */

		static CoordRec[] char39_stroke0 = {
			new CoordRec(52.381F, 100F),
			new CoordRec(52.381F, 66.6667F)

		};

		static StrokeRec[] char39 = { new StrokeRec(2, char39_stroke0) };
		// char: 40 '(' */

		static CoordRec[] char40_stroke0 = {
			new CoordRec(69.0476F, 119.048F),
			new CoordRec(59.5238F, 109.524F),
			new CoordRec(50F, 95.2381F),
			new CoordRec(40.4762F, 76.1905F),
			new CoordRec(35.7143F, 52.381F),
			new CoordRec(35.7143F, 33.3333F),
			new CoordRec(40.4762F, 9.5238F),
			new CoordRec(50F, -9.5238F),
			new CoordRec(59.5238F, -23.8095F),
			new CoordRec(69.0476F, -33.3333F)

		};

		static StrokeRec[] char40 = { new StrokeRec(10, char40_stroke0) };
		// char: 41 ')' */

		static CoordRec[] char41_stroke0 = {
			new CoordRec(35.7143F, 119.048F),
			new CoordRec(45.2381F, 109.524F),
			new CoordRec(54.7619F, 95.2381F),
			new CoordRec(64.2857F, 76.1905F),
			new CoordRec(69.0476F, 52.381F),
			new CoordRec(69.0476F, 33.3333F),
			new CoordRec(64.2857F, 9.5238F),
			new CoordRec(54.7619F, -9.5238F),
			new CoordRec(45.2381F, -23.8095F),
			new CoordRec(35.7143F, -33.3333F)

		};

		static StrokeRec[] char41 = { new StrokeRec(10, char41_stroke0) };
		// char: 42 '*' */

		static CoordRec[] char42_stroke0 = {
			new CoordRec(52.381F, 71.4286F),
			new CoordRec(52.381F, 14.2857F)

		};
		static CoordRec[] char42_stroke1 = {
			new CoordRec(28.5715F, 57.1429F),
			new CoordRec(76.1905F, 28.5714F)

		};
		static CoordRec[] char42_stroke2 = {
			new CoordRec(76.1905F, 57.1429F),
			new CoordRec(28.5715F, 28.5714F)

		};
		static StrokeRec[] char42 = {
			new StrokeRec(2, char42_stroke0),
			new StrokeRec(2, char42_stroke1),
			new StrokeRec(2, char42_stroke2)

		};
		// char: 43 '+' */

		static CoordRec[] char43_stroke0 = {
			new CoordRec(52.3809F, 85.7143F),
			new CoordRec(52.3809F, 0F)

		};
		static CoordRec[] char43_stroke1 = {
			new CoordRec(9.5238F, 42.8571F),
			new CoordRec(95.2381F, 42.8571F)

		};
		static StrokeRec[] char43 = {
			new StrokeRec(2, char43_stroke0),
			new StrokeRec(2, char43_stroke1)

		};
		// char: 44 ',' */

		static CoordRec[] char44_stroke0 = {
			new CoordRec(57.1429F, 4.7619F),
			new CoordRec(52.381F, 0F),
			new CoordRec(47.6191F, 4.7619F),
			new CoordRec(52.381F, 9.5238F),
			new CoordRec(57.1429F, 4.7619F),
			new CoordRec(57.1429F, -4.7619F),
			new CoordRec(52.381F, -14.2857F),
			new CoordRec(47.6191F, -19.0476F)

		};

		static StrokeRec[] char44 = { new StrokeRec(8, char44_stroke0) };
		// char: 45 '-' */

		static CoordRec[] char45_stroke0 = {
			new CoordRec(9.5238F, 42.8571F),
			new CoordRec(95.2381F, 42.8571F)

		};

		static StrokeRec[] char45 = { new StrokeRec(2, char45_stroke0) };
		// char: 46 '.' */

		static CoordRec[] char46_stroke0 = {
			new CoordRec(52.381F, 9.5238F),
			new CoordRec(47.6191F, 4.7619F),
			new CoordRec(52.381F, 0F),
			new CoordRec(57.1429F, 4.7619F),
			new CoordRec(52.381F, 9.5238F)

		};

		static StrokeRec[] char46 = { new StrokeRec(5, char46_stroke0) };
		// char: 47 '/' */

		static CoordRec[] char47_stroke0 = {
			new CoordRec(19.0476F, -14.2857F),
			new CoordRec(85.7143F, 100F)

		};

		static StrokeRec[] char47 = { new StrokeRec(2, char47_stroke0) };
		// char: 48 '0' */

		static CoordRec[] char48_stroke0 = {
			new CoordRec(47.619F, 100F),
			new CoordRec(33.3333F, 95.2381F),
			new CoordRec(23.8095F, 80.9524F),
			new CoordRec(19.0476F, 57.1429F),
			new CoordRec(19.0476F, 42.8571F),
			new CoordRec(23.8095F, 19.0476F),
			new CoordRec(33.3333F, 4.7619F),
			new CoordRec(47.619F, 0F),
			new CoordRec(57.1428F, 0F),
			new CoordRec(71.4286F, 4.7619F),
			new CoordRec(80.9524F, 19.0476F),
			new CoordRec(85.7143F, 42.8571F),
			new CoordRec(85.7143F, 57.1429F),
			new CoordRec(80.9524F, 80.9524F),
			new CoordRec(71.4286F, 95.2381F),
			new CoordRec(57.1428F, 100F),
			new CoordRec(47.619F, 100F)

		};

		static StrokeRec[] char48 = { new StrokeRec(17, char48_stroke0) };
		// char: 49 '1' */

		static CoordRec[] char49_stroke0 = {
			new CoordRec(40.4762F, 80.9524F),
			new CoordRec(50F, 85.7143F),
			new CoordRec(64.2857F, 100F),
			new CoordRec(64.2857F, 0F)

		};

		static StrokeRec[] char49 = { new StrokeRec(4, char49_stroke0) };
		// char: 50 '2' */

		static CoordRec[] char50_stroke0 = {
			new CoordRec(23.8095F, 76.1905F),
			new CoordRec(23.8095F, 80.9524F),
			new CoordRec(28.5714F, 90.4762F),
			new CoordRec(33.3333F, 95.2381F),
			new CoordRec(42.8571F, 100F),
			new CoordRec(61.9047F, 100F),
			new CoordRec(71.4286F, 95.2381F),
			new CoordRec(76.1905F, 90.4762F),
			new CoordRec(80.9524F, 80.9524F),
			new CoordRec(80.9524F, 71.4286F),
			new CoordRec(76.1905F, 61.9048F),
			new CoordRec(66.6666F, 47.619F),
			new CoordRec(19.0476F, 0F),
			new CoordRec(85.7143F, 0F)

		};

		static StrokeRec[] char50 = { new StrokeRec(14, char50_stroke0) };
		// char: 51 '3' */

		static CoordRec[] char51_stroke0 = {
			new CoordRec(28.5714F, 100F),
			new CoordRec(80.9524F, 100F),
			new CoordRec(52.3809F, 61.9048F),
			new CoordRec(66.6666F, 61.9048F),
			new CoordRec(76.1905F, 57.1429F),
			new CoordRec(80.9524F, 52.381F),
			new CoordRec(85.7143F, 38.0952F),
			new CoordRec(85.7143F, 28.5714F),
			new CoordRec(80.9524F, 14.2857F),
			new CoordRec(71.4286F, 4.7619F),
			new CoordRec(57.1428F, 0F),
			new CoordRec(42.8571F, 0F),
			new CoordRec(28.5714F, 4.7619F),
			new CoordRec(23.8095F, 9.5238F),
			new CoordRec(19.0476F, 19.0476F)

		};

		static StrokeRec[] char51 = { new StrokeRec(15, char51_stroke0) };
		// char: 52 '4' */

		static CoordRec[] char52_stroke0 = {
			new CoordRec(64.2857F, 100F),
			new CoordRec(16.6667F, 33.3333F),
			new CoordRec(88.0952F, 33.3333F)

		};
		static CoordRec[] char52_stroke1 = {
			new CoordRec(64.2857F, 100F),
			new CoordRec(64.2857F, 0F)

		};
		static StrokeRec[] char52 = {
			new StrokeRec(3, char52_stroke0),
			new StrokeRec(2, char52_stroke1)

		};
		// char: 53 '5' */

		static CoordRec[] char53_stroke0 = {
			new CoordRec(76.1905F, 100F),
			new CoordRec(28.5714F, 100F),
			new CoordRec(23.8095F, 57.1429F),
			new CoordRec(28.5714F, 61.9048F),
			new CoordRec(42.8571F, 66.6667F),
			new CoordRec(57.1428F, 66.6667F),
			new CoordRec(71.4286F, 61.9048F),
			new CoordRec(80.9524F, 52.381F),
			new CoordRec(85.7143F, 38.0952F),
			new CoordRec(85.7143F, 28.5714F),
			new CoordRec(80.9524F, 14.2857F),
			new CoordRec(71.4286F, 4.7619F),
			new CoordRec(57.1428F, 0F),
			new CoordRec(42.8571F, 0F),
			new CoordRec(28.5714F, 4.7619F),
			new CoordRec(23.8095F, 9.5238F),
			new CoordRec(19.0476F, 19.0476F)

		};

		static StrokeRec[] char53 = { new StrokeRec(17, char53_stroke0) };
		// char: 54 '6' */

		static CoordRec[] char54_stroke0 = {
			new CoordRec(78.5714F, 85.7143F),
			new CoordRec(73.8096F, 95.2381F),
			new CoordRec(59.5238F, 100F),
			new CoordRec(50F, 100F),
			new CoordRec(35.7143F, 95.2381F),
			new CoordRec(26.1905F, 80.9524F),
			new CoordRec(21.4286F, 57.1429F),
			new CoordRec(21.4286F, 33.3333F),
			new CoordRec(26.1905F, 14.2857F),
			new CoordRec(35.7143F, 4.7619F),
			new CoordRec(50F, 0F),
			new CoordRec(54.7619F, 0F),
			new CoordRec(69.0476F, 4.7619F),
			new CoordRec(78.5714F, 14.2857F),
			new CoordRec(83.3334F, 28.5714F),
			new CoordRec(83.3334F, 33.3333F),
			new CoordRec(78.5714F, 47.619F),
			new CoordRec(69.0476F, 57.1429F),
			new CoordRec(54.7619F, 61.9048F),
			new CoordRec(50F, 61.9048F),
			new CoordRec(35.7143F, 57.1429F),
			new CoordRec(26.1905F, 47.619F),
			new CoordRec(21.4286F, 33.3333F)

		};

		static StrokeRec[] char54 = { new StrokeRec(23, char54_stroke0) };
		// char: 55 '7' */

		static CoordRec[] char55_stroke0 = {
			new CoordRec(85.7143F, 100F),
			new CoordRec(38.0952F, 0F)

		};
		static CoordRec[] char55_stroke1 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(85.7143F, 100F)

		};
		static StrokeRec[] char55 = {
			new StrokeRec(2, char55_stroke0),
			new StrokeRec(2, char55_stroke1)

		};
		// char: 56 '8' */

		static CoordRec[] char56_stroke0 = {
			new CoordRec(42.8571F, 100F),
			new CoordRec(28.5714F, 95.2381F),
			new CoordRec(23.8095F, 85.7143F),
			new CoordRec(23.8095F, 76.1905F),
			new CoordRec(28.5714F, 66.6667F),
			new CoordRec(38.0952F, 61.9048F),
			new CoordRec(57.1428F, 57.1429F),
			new CoordRec(71.4286F, 52.381F),
			new CoordRec(80.9524F, 42.8571F),
			new CoordRec(85.7143F, 33.3333F),
			new CoordRec(85.7143F, 19.0476F),
			new CoordRec(80.9524F, 9.5238F),
			new CoordRec(76.1905F, 4.7619F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(42.8571F, 0F),
			new CoordRec(28.5714F, 4.7619F),
			new CoordRec(23.8095F, 9.5238F),
			new CoordRec(19.0476F, 19.0476F),
			new CoordRec(19.0476F, 33.3333F),
			new CoordRec(23.8095F, 42.8571F),
			new CoordRec(33.3333F, 52.381F),
			new CoordRec(47.619F, 57.1429F),
			new CoordRec(66.6666F, 61.9048F),
			new CoordRec(76.1905F, 66.6667F),
			new CoordRec(80.9524F, 76.1905F),
			new CoordRec(80.9524F, 85.7143F),
			new CoordRec(76.1905F, 95.2381F),
			new CoordRec(61.9047F, 100F),
			new CoordRec(42.8571F, 100F)

		};

		static StrokeRec[] char56 = { new StrokeRec(29, char56_stroke0) };
		// char: 57 '9' */

		static CoordRec[] char57_stroke0 = {
			new CoordRec(83.3334F, 66.6667F),
			new CoordRec(78.5714F, 52.381F),
			new CoordRec(69.0476F, 42.8571F),
			new CoordRec(54.7619F, 38.0952F),
			new CoordRec(50F, 38.0952F),
			new CoordRec(35.7143F, 42.8571F),
			new CoordRec(26.1905F, 52.381F),
			new CoordRec(21.4286F, 66.6667F),
			new CoordRec(21.4286F, 71.4286F),
			new CoordRec(26.1905F, 85.7143F),
			new CoordRec(35.7143F, 95.2381F),
			new CoordRec(50F, 100F),
			new CoordRec(54.7619F, 100F),
			new CoordRec(69.0476F, 95.2381F),
			new CoordRec(78.5714F, 85.7143F),
			new CoordRec(83.3334F, 66.6667F),
			new CoordRec(83.3334F, 42.8571F),
			new CoordRec(78.5714F, 19.0476F),
			new CoordRec(69.0476F, 4.7619F),
			new CoordRec(54.7619F, 0F),
			new CoordRec(45.2381F, 0F),
			new CoordRec(30.9524F, 4.7619F),
			new CoordRec(26.1905F, 14.2857F)

		};

		static StrokeRec[] char57 = { new StrokeRec(23, char57_stroke0) };
		// char: 58 ':' */

		static CoordRec[] char58_stroke0 = {
			new CoordRec(52.381F, 66.6667F),
			new CoordRec(47.6191F, 61.9048F),
			new CoordRec(52.381F, 57.1429F),
			new CoordRec(57.1429F, 61.9048F),
			new CoordRec(52.381F, 66.6667F)

		};
		static CoordRec[] char58_stroke1 = {
			new CoordRec(52.381F, 9.5238F),
			new CoordRec(47.6191F, 4.7619F),
			new CoordRec(52.381F, 0F),
			new CoordRec(57.1429F, 4.7619F),
			new CoordRec(52.381F, 9.5238F)

		};
		static StrokeRec[] char58 = {
			new StrokeRec(5, char58_stroke0),
			new StrokeRec(5, char58_stroke1)

		};
		// char: 59 ';' */

		static CoordRec[] char59_stroke0 = {
			new CoordRec(52.381F, 66.6667F),
			new CoordRec(47.6191F, 61.9048F),
			new CoordRec(52.381F, 57.1429F),
			new CoordRec(57.1429F, 61.9048F),
			new CoordRec(52.381F, 66.6667F)

		};
		static CoordRec[] char59_stroke1 = {
			new CoordRec(57.1429F, 4.7619F),
			new CoordRec(52.381F, 0F),
			new CoordRec(47.6191F, 4.7619F),
			new CoordRec(52.381F, 9.5238F),
			new CoordRec(57.1429F, 4.7619F),
			new CoordRec(57.1429F, -4.7619F),
			new CoordRec(52.381F, -14.2857F),
			new CoordRec(47.6191F, -19.0476F)

		};
		static StrokeRec[] char59 = {
			new StrokeRec(5, char59_stroke0),
			new StrokeRec(8, char59_stroke1)

		};
		// char: 60 '<' */

		static CoordRec[] char60_stroke0 = {
			new CoordRec(90.4762F, 85.7143F),
			new CoordRec(14.2857F, 42.8571F),
			new CoordRec(90.4762F, 0F)

		};

		static StrokeRec[] char60 = { new StrokeRec(3, char60_stroke0) };
		// char: 61 '=' */

		static CoordRec[] char61_stroke0 = {
			new CoordRec(9.5238F, 57.1429F),
			new CoordRec(95.2381F, 57.1429F)

		};
		static CoordRec[] char61_stroke1 = {
			new CoordRec(9.5238F, 28.5714F),
			new CoordRec(95.2381F, 28.5714F)

		};
		static StrokeRec[] char61 = {
			new StrokeRec(2, char61_stroke0),
			new StrokeRec(2, char61_stroke1)

		};
		// char: 62 '>' */

		static CoordRec[] char62_stroke0 = {
			new CoordRec(14.2857F, 85.7143F),
			new CoordRec(90.4762F, 42.8571F),
			new CoordRec(14.2857F, 0F)

		};

		static StrokeRec[] char62 = { new StrokeRec(3, char62_stroke0) };
		// char: 63 '?' */

		static CoordRec[] char63_stroke0 = {
			new CoordRec(23.8095F, 76.1905F),
			new CoordRec(23.8095F, 80.9524F),
			new CoordRec(28.5714F, 90.4762F),
			new CoordRec(33.3333F, 95.2381F),
			new CoordRec(42.8571F, 100F),
			new CoordRec(61.9047F, 100F),
			new CoordRec(71.4285F, 95.2381F),
			new CoordRec(76.1905F, 90.4762F),
			new CoordRec(80.9524F, 80.9524F),
			new CoordRec(80.9524F, 71.4286F),
			new CoordRec(76.1905F, 61.9048F),
			new CoordRec(71.4285F, 57.1429F),
			new CoordRec(52.3809F, 47.619F),
			new CoordRec(52.3809F, 33.3333F)

		};
		static CoordRec[] char63_stroke1 = {
			new CoordRec(52.3809F, 9.5238F),
			new CoordRec(47.619F, 4.7619F),
			new CoordRec(52.3809F, 0F),
			new CoordRec(57.1428F, 4.7619F),
			new CoordRec(52.3809F, 9.5238F)

		};
		static StrokeRec[] char63 = {
			new StrokeRec(14, char63_stroke0),
			new StrokeRec(5, char63_stroke1)

		};
		// char: 64 '@' */

		static CoordRec[] char64_stroke0 = {
			new CoordRec(64.2857F, 52.381F),
			new CoordRec(54.7619F, 57.1429F),
			new CoordRec(45.2381F, 57.1429F),
			new CoordRec(40.4762F, 47.619F),
			new CoordRec(40.4762F, 42.8571F),
			new CoordRec(45.2381F, 33.3333F),
			new CoordRec(54.7619F, 33.3333F),
			new CoordRec(64.2857F, 38.0952F)

		};
		static CoordRec[] char64_stroke1 = {
			new CoordRec(64.2857F, 57.1429F),
			new CoordRec(64.2857F, 38.0952F),
			new CoordRec(69.0476F, 33.3333F),
			new CoordRec(78.5714F, 33.3333F),
			new CoordRec(83.3334F, 42.8571F),
			new CoordRec(83.3334F, 47.619F),
			new CoordRec(78.5714F, 61.9048F),
			new CoordRec(69.0476F, 71.4286F),
			new CoordRec(54.7619F, 76.1905F),
			new CoordRec(50F, 76.1905F),
			new CoordRec(35.7143F, 71.4286F),
			new CoordRec(26.1905F, 61.9048F),
			new CoordRec(21.4286F, 47.619F),
			new CoordRec(21.4286F, 42.8571F),
			new CoordRec(26.1905F, 28.5714F),
			new CoordRec(35.7143F, 19.0476F),
			new CoordRec(50F, 14.2857F),
			new CoordRec(54.7619F, 14.2857F),
			new CoordRec(69.0476F, 19.0476F)

		};
		static StrokeRec[] char64 = {
			new StrokeRec(8, char64_stroke0),
			new StrokeRec(19, char64_stroke1)

		};
		// char: 65 'A' */

		static CoordRec[] char65_stroke0 = {
			new CoordRec(52.3809F, 100F),
			new CoordRec(14.2857F, 0F)

		};
		static CoordRec[] char65_stroke1 = {
			new CoordRec(52.3809F, 100F),
			new CoordRec(90.4762F, 0F)

		};
		static CoordRec[] char65_stroke2 = {
			new CoordRec(28.5714F, 33.3333F),
			new CoordRec(76.1905F, 33.3333F)

		};
		static StrokeRec[] char65 = {
			new StrokeRec(2, char65_stroke0),
			new StrokeRec(2, char65_stroke1),
			new StrokeRec(2, char65_stroke2)

		};
		// char: 66 'B' */

		static CoordRec[] char66_stroke0 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(19.0476F, 0F)

		};
		static CoordRec[] char66_stroke1 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(61.9047F, 100F),
			new CoordRec(76.1905F, 95.2381F),
			new CoordRec(80.9524F, 90.4762F),
			new CoordRec(85.7143F, 80.9524F),
			new CoordRec(85.7143F, 71.4286F),
			new CoordRec(80.9524F, 61.9048F),
			new CoordRec(76.1905F, 57.1429F),
			new CoordRec(61.9047F, 52.381F)

		};
		static CoordRec[] char66_stroke2 = {
			new CoordRec(19.0476F, 52.381F),
			new CoordRec(61.9047F, 52.381F),
			new CoordRec(76.1905F, 47.619F),
			new CoordRec(80.9524F, 42.8571F),
			new CoordRec(85.7143F, 33.3333F),
			new CoordRec(85.7143F, 19.0476F),
			new CoordRec(80.9524F, 9.5238F),
			new CoordRec(76.1905F, 4.7619F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(19.0476F, 0F)

		};
		static StrokeRec[] char66 = {
			new StrokeRec(2, char66_stroke0),
			new StrokeRec(9, char66_stroke1),
			new StrokeRec(10, char66_stroke2)

		};
		// char: 67 'C' */

		static CoordRec[] char67_stroke0 = {
			new CoordRec(88.0952F, 76.1905F),
			new CoordRec(83.3334F, 85.7143F),
			new CoordRec(73.8096F, 95.2381F),
			new CoordRec(64.2857F, 100F),
			new CoordRec(45.2381F, 100F),
			new CoordRec(35.7143F, 95.2381F),
			new CoordRec(26.1905F, 85.7143F),
			new CoordRec(21.4286F, 76.1905F),
			new CoordRec(16.6667F, 61.9048F),
			new CoordRec(16.6667F, 38.0952F),
			new CoordRec(21.4286F, 23.8095F),
			new CoordRec(26.1905F, 14.2857F),
			new CoordRec(35.7143F, 4.7619F),
			new CoordRec(45.2381F, 0F),
			new CoordRec(64.2857F, 0F),
			new CoordRec(73.8096F, 4.7619F),
			new CoordRec(83.3334F, 14.2857F),
			new CoordRec(88.0952F, 23.8095F)

		};

		static StrokeRec[] char67 = { new StrokeRec(18, char67_stroke0) };
		// char: 68 'D' */

		static CoordRec[] char68_stroke0 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(19.0476F, 0F)

		};
		static CoordRec[] char68_stroke1 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(52.3809F, 100F),
			new CoordRec(66.6666F, 95.2381F),
			new CoordRec(76.1905F, 85.7143F),
			new CoordRec(80.9524F, 76.1905F),
			new CoordRec(85.7143F, 61.9048F),
			new CoordRec(85.7143F, 38.0952F),
			new CoordRec(80.9524F, 23.8095F),
			new CoordRec(76.1905F, 14.2857F),
			new CoordRec(66.6666F, 4.7619F),
			new CoordRec(52.3809F, 0F),
			new CoordRec(19.0476F, 0F)

		};
		static StrokeRec[] char68 = {
			new StrokeRec(2, char68_stroke0),
			new StrokeRec(12, char68_stroke1)

		};
		// char: 69 'E' */

		static CoordRec[] char69_stroke0 = {
			new CoordRec(21.4286F, 100F),
			new CoordRec(21.4286F, 0F)

		};
		static CoordRec[] char69_stroke1 = {
			new CoordRec(21.4286F, 100F),
			new CoordRec(83.3334F, 100F)

		};
		static CoordRec[] char69_stroke2 = {
			new CoordRec(21.4286F, 52.381F),
			new CoordRec(59.5238F, 52.381F)

		};
		static CoordRec[] char69_stroke3 = {
			new CoordRec(21.4286F, 0F),
			new CoordRec(83.3334F, 0F)

		};
		static StrokeRec[] char69 = {
			new StrokeRec(2, char69_stroke0),
			new StrokeRec(2, char69_stroke1),
			new StrokeRec(2, char69_stroke2),
			new StrokeRec(2, char69_stroke3)

		};
		// char: 70 'F' */

		static CoordRec[] char70_stroke0 = {
			new CoordRec(21.4286F, 100F),
			new CoordRec(21.4286F, 0F)

		};
		static CoordRec[] char70_stroke1 = {
			new CoordRec(21.4286F, 100F),
			new CoordRec(83.3334F, 100F)

		};
		static CoordRec[] char70_stroke2 = {
			new CoordRec(21.4286F, 52.381F),
			new CoordRec(59.5238F, 52.381F)

		};
		static StrokeRec[] char70 = {
			new StrokeRec(2, char70_stroke0),
			new StrokeRec(2, char70_stroke1),
			new StrokeRec(2, char70_stroke2)

		};
		// char: 71 'G' */

		static CoordRec[] char71_stroke0 = {
			new CoordRec(88.0952F, 76.1905F),
			new CoordRec(83.3334F, 85.7143F),
			new CoordRec(73.8096F, 95.2381F),
			new CoordRec(64.2857F, 100F),
			new CoordRec(45.2381F, 100F),
			new CoordRec(35.7143F, 95.2381F),
			new CoordRec(26.1905F, 85.7143F),
			new CoordRec(21.4286F, 76.1905F),
			new CoordRec(16.6667F, 61.9048F),
			new CoordRec(16.6667F, 38.0952F),
			new CoordRec(21.4286F, 23.8095F),
			new CoordRec(26.1905F, 14.2857F),
			new CoordRec(35.7143F, 4.7619F),
			new CoordRec(45.2381F, 0F),
			new CoordRec(64.2857F, 0F),
			new CoordRec(73.8096F, 4.7619F),
			new CoordRec(83.3334F, 14.2857F),
			new CoordRec(88.0952F, 23.8095F),
			new CoordRec(88.0952F, 38.0952F)

		};
		static CoordRec[] char71_stroke1 = {
			new CoordRec(64.2857F, 38.0952F),
			new CoordRec(88.0952F, 38.0952F)

		};
		static StrokeRec[] char71 = {
			new StrokeRec(19, char71_stroke0),
			new StrokeRec(2, char71_stroke1)

		};
		// char: 72 'H' */

		static CoordRec[] char72_stroke0 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(19.0476F, 0F)

		};
		static CoordRec[] char72_stroke1 = {
			new CoordRec(85.7143F, 100F),
			new CoordRec(85.7143F, 0F)

		};
		static CoordRec[] char72_stroke2 = {
			new CoordRec(19.0476F, 52.381F),
			new CoordRec(85.7143F, 52.381F)

		};
		static StrokeRec[] char72 = {
			new StrokeRec(2, char72_stroke0),
			new StrokeRec(2, char72_stroke1),
			new StrokeRec(2, char72_stroke2)

		};
		// char: 73 'I' */

		static CoordRec[] char73_stroke0 = {
			new CoordRec(52.381F, 100F),
			new CoordRec(52.381F, 0F)

		};

		static StrokeRec[] char73 = { new StrokeRec(2, char73_stroke0) };
		// char: 74 'J' */

		static CoordRec[] char74_stroke0 = {
			new CoordRec(76.1905F, 100F),
			new CoordRec(76.1905F, 23.8095F),
			new CoordRec(71.4286F, 9.5238F),
			new CoordRec(66.6667F, 4.7619F),
			new CoordRec(57.1429F, 0F),
			new CoordRec(47.6191F, 0F),
			new CoordRec(38.0953F, 4.7619F),
			new CoordRec(33.3334F, 9.5238F),
			new CoordRec(28.5715F, 23.8095F),
			new CoordRec(28.5715F, 33.3333F)

		};

		static StrokeRec[] char74 = { new StrokeRec(10, char74_stroke0) };
		// char: 75 'K' */

		static CoordRec[] char75_stroke0 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(19.0476F, 0F)

		};
		static CoordRec[] char75_stroke1 = {
			new CoordRec(85.7143F, 100F),
			new CoordRec(19.0476F, 33.3333F)

		};
		static CoordRec[] char75_stroke2 = {
			new CoordRec(42.8571F, 57.1429F),
			new CoordRec(85.7143F, 0F)

		};
		static StrokeRec[] char75 = {
			new StrokeRec(2, char75_stroke0),
			new StrokeRec(2, char75_stroke1),
			new StrokeRec(2, char75_stroke2)

		};
		// char: 76 'L' */

		static CoordRec[] char76_stroke0 = {
			new CoordRec(23.8095F, 100F),
			new CoordRec(23.8095F, 0F)

		};
		static CoordRec[] char76_stroke1 = {
			new CoordRec(23.8095F, 0F),
			new CoordRec(80.9524F, 0F)

		};
		static StrokeRec[] char76 = {
			new StrokeRec(2, char76_stroke0),
			new StrokeRec(2, char76_stroke1)

		};
		// char: 77 'M' */

		static CoordRec[] char77_stroke0 = {
			new CoordRec(14.2857F, 100F),
			new CoordRec(14.2857F, 0F)

		};
		static CoordRec[] char77_stroke1 = {
			new CoordRec(14.2857F, 100F),
			new CoordRec(52.3809F, 0F)

		};
		static CoordRec[] char77_stroke2 = {
			new CoordRec(90.4762F, 100F),
			new CoordRec(52.3809F, 0F)

		};
		static CoordRec[] char77_stroke3 = {
			new CoordRec(90.4762F, 100F),
			new CoordRec(90.4762F, 0F)

		};
		static StrokeRec[] char77 = {
			new StrokeRec(2, char77_stroke0),
			new StrokeRec(2, char77_stroke1),
			new StrokeRec(2, char77_stroke2),
			new StrokeRec(2, char77_stroke3)

		};
		// char: 78 'N' */

		static CoordRec[] char78_stroke0 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(19.0476F, 0F)

		};
		static CoordRec[] char78_stroke1 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(85.7143F, 0F)

		};
		static CoordRec[] char78_stroke2 = {
			new CoordRec(85.7143F, 100F),
			new CoordRec(85.7143F, 0F)

		};
		static StrokeRec[] char78 = {
			new StrokeRec(2, char78_stroke0),
			new StrokeRec(2, char78_stroke1),
			new StrokeRec(2, char78_stroke2)

		};
		// char: 79 'O' */

		static CoordRec[] char79_stroke0 = {
			new CoordRec(42.8571F, 100F),
			new CoordRec(33.3333F, 95.2381F),
			new CoordRec(23.8095F, 85.7143F),
			new CoordRec(19.0476F, 76.1905F),
			new CoordRec(14.2857F, 61.9048F),
			new CoordRec(14.2857F, 38.0952F),
			new CoordRec(19.0476F, 23.8095F),
			new CoordRec(23.8095F, 14.2857F),
			new CoordRec(33.3333F, 4.7619F),
			new CoordRec(42.8571F, 0F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(71.4286F, 4.7619F),
			new CoordRec(80.9524F, 14.2857F),
			new CoordRec(85.7143F, 23.8095F),
			new CoordRec(90.4762F, 38.0952F),
			new CoordRec(90.4762F, 61.9048F),
			new CoordRec(85.7143F, 76.1905F),
			new CoordRec(80.9524F, 85.7143F),
			new CoordRec(71.4286F, 95.2381F),
			new CoordRec(61.9047F, 100F),
			new CoordRec(42.8571F, 100F)

		};

		static StrokeRec[] char79 = { new StrokeRec(21, char79_stroke0) };
		// char: 80 'P' */

		static CoordRec[] char80_stroke0 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(19.0476F, 0F)

		};
		static CoordRec[] char80_stroke1 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(61.9047F, 100F),
			new CoordRec(76.1905F, 95.2381F),
			new CoordRec(80.9524F, 90.4762F),
			new CoordRec(85.7143F, 80.9524F),
			new CoordRec(85.7143F, 66.6667F),
			new CoordRec(80.9524F, 57.1429F),
			new CoordRec(76.1905F, 52.381F),
			new CoordRec(61.9047F, 47.619F),
			new CoordRec(19.0476F, 47.619F)

		};
		static StrokeRec[] char80 = {
			new StrokeRec(2, char80_stroke0),
			new StrokeRec(10, char80_stroke1)

		};
		// char: 81 'Q' */

		static CoordRec[] char81_stroke0 = {
			new CoordRec(42.8571F, 100F),
			new CoordRec(33.3333F, 95.2381F),
			new CoordRec(23.8095F, 85.7143F),
			new CoordRec(19.0476F, 76.1905F),
			new CoordRec(14.2857F, 61.9048F),
			new CoordRec(14.2857F, 38.0952F),
			new CoordRec(19.0476F, 23.8095F),
			new CoordRec(23.8095F, 14.2857F),
			new CoordRec(33.3333F, 4.7619F),
			new CoordRec(42.8571F, 0F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(71.4286F, 4.7619F),
			new CoordRec(80.9524F, 14.2857F),
			new CoordRec(85.7143F, 23.8095F),
			new CoordRec(90.4762F, 38.0952F),
			new CoordRec(90.4762F, 61.9048F),
			new CoordRec(85.7143F, 76.1905F),
			new CoordRec(80.9524F, 85.7143F),
			new CoordRec(71.4286F, 95.2381F),
			new CoordRec(61.9047F, 100F),
			new CoordRec(42.8571F, 100F)

		};
		static CoordRec[] char81_stroke1 = {
			new CoordRec(57.1428F, 19.0476F),
			new CoordRec(85.7143F, -9.5238F)

		};
		static StrokeRec[] char81 = {
			new StrokeRec(21, char81_stroke0),
			new StrokeRec(2, char81_stroke1)

		};
		// char: 82 'R' */

		static CoordRec[] char82_stroke0 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(19.0476F, 0F)

		};
		static CoordRec[] char82_stroke1 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(61.9047F, 100F),
			new CoordRec(76.1905F, 95.2381F),
			new CoordRec(80.9524F, 90.4762F),
			new CoordRec(85.7143F, 80.9524F),
			new CoordRec(85.7143F, 71.4286F),
			new CoordRec(80.9524F, 61.9048F),
			new CoordRec(76.1905F, 57.1429F),
			new CoordRec(61.9047F, 52.381F),
			new CoordRec(19.0476F, 52.381F)

		};
		static CoordRec[] char82_stroke2 = {
			new CoordRec(52.3809F, 52.381F),
			new CoordRec(85.7143F, 0F)

		};
		static StrokeRec[] char82 = {
			new StrokeRec(2, char82_stroke0),
			new StrokeRec(10, char82_stroke1),
			new StrokeRec(2, char82_stroke2)

		};
		// char: 83 'S' */

		static CoordRec[] char83_stroke0 = {
			new CoordRec(85.7143F, 85.7143F),
			new CoordRec(76.1905F, 95.2381F),
			new CoordRec(61.9047F, 100F),
			new CoordRec(42.8571F, 100F),
			new CoordRec(28.5714F, 95.2381F),
			new CoordRec(19.0476F, 85.7143F),
			new CoordRec(19.0476F, 76.1905F),
			new CoordRec(23.8095F, 66.6667F),
			new CoordRec(28.5714F, 61.9048F),
			new CoordRec(38.0952F, 57.1429F),
			new CoordRec(66.6666F, 47.619F),
			new CoordRec(76.1905F, 42.8571F),
			new CoordRec(80.9524F, 38.0952F),
			new CoordRec(85.7143F, 28.5714F),
			new CoordRec(85.7143F, 14.2857F),
			new CoordRec(76.1905F, 4.7619F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(42.8571F, 0F),
			new CoordRec(28.5714F, 4.7619F),
			new CoordRec(19.0476F, 14.2857F)

		};

		static StrokeRec[] char83 = { new StrokeRec(20, char83_stroke0) };
		// char: 84 'T' */

		static CoordRec[] char84_stroke0 = {
			new CoordRec(52.3809F, 100F),
			new CoordRec(52.3809F, 0F)

		};
		static CoordRec[] char84_stroke1 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(85.7143F, 100F)

		};
		static StrokeRec[] char84 = {
			new StrokeRec(2, char84_stroke0),
			new StrokeRec(2, char84_stroke1)

		};
		// char: 85 'U' */

		static CoordRec[] char85_stroke0 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(19.0476F, 28.5714F),
			new CoordRec(23.8095F, 14.2857F),
			new CoordRec(33.3333F, 4.7619F),
			new CoordRec(47.619F, 0F),
			new CoordRec(57.1428F, 0F),
			new CoordRec(71.4286F, 4.7619F),
			new CoordRec(80.9524F, 14.2857F),
			new CoordRec(85.7143F, 28.5714F),
			new CoordRec(85.7143F, 100F)

		};

		static StrokeRec[] char85 = { new StrokeRec(10, char85_stroke0) };
		// char: 86 'V' */

		static CoordRec[] char86_stroke0 = {
			new CoordRec(14.2857F, 100F),
			new CoordRec(52.3809F, 0F)

		};
		static CoordRec[] char86_stroke1 = {
			new CoordRec(90.4762F, 100F),
			new CoordRec(52.3809F, 0F)

		};
		static StrokeRec[] char86 = {
			new StrokeRec(2, char86_stroke0),
			new StrokeRec(2, char86_stroke1)

		};
		// char: 87 'W' */

		static CoordRec[] char87_stroke0 = {
			new CoordRec(4.7619F, 100F),
			new CoordRec(28.5714F, 0F)

		};
		static CoordRec[] char87_stroke1 = {
			new CoordRec(52.3809F, 100F),
			new CoordRec(28.5714F, 0F)

		};
		static CoordRec[] char87_stroke2 = {
			new CoordRec(52.3809F, 100F),
			new CoordRec(76.1905F, 0F)

		};
		static CoordRec[] char87_stroke3 = {
			new CoordRec(100F, 100F),
			new CoordRec(76.1905F, 0F)

		};
		static StrokeRec[] char87 = {
			new StrokeRec(2, char87_stroke0),
			new StrokeRec(2, char87_stroke1),
			new StrokeRec(2, char87_stroke2),
			new StrokeRec(2, char87_stroke3)

		};
		// char: 88 'X' */

		static CoordRec[] char88_stroke0 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(85.7143F, 0F)

		};
		static CoordRec[] char88_stroke1 = {
			new CoordRec(85.7143F, 100F),
			new CoordRec(19.0476F, 0F)

		};
		static StrokeRec[] char88 = {
			new StrokeRec(2, char88_stroke0),
			new StrokeRec(2, char88_stroke1)

		};
		// char: 89 'Y' */

		static CoordRec[] char89_stroke0 = {
			new CoordRec(14.2857F, 100F),
			new CoordRec(52.3809F, 52.381F),
			new CoordRec(52.3809F, 0F)

		};
		static CoordRec[] char89_stroke1 = {
			new CoordRec(90.4762F, 100F),
			new CoordRec(52.3809F, 52.381F)

		};
		static StrokeRec[] char89 = {
			new StrokeRec(3, char89_stroke0),
			new StrokeRec(2, char89_stroke1)

		};
		// char: 90 'Z' */

		static CoordRec[] char90_stroke0 = {
			new CoordRec(85.7143F, 100F),
			new CoordRec(19.0476F, 0F)

		};
		static CoordRec[] char90_stroke1 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(85.7143F, 100F)

		};
		static CoordRec[] char90_stroke2 = {
			new CoordRec(19.0476F, 0F),
			new CoordRec(85.7143F, 0F)

		};
		static StrokeRec[] char90 = {
			new StrokeRec(2, char90_stroke0),
			new StrokeRec(2, char90_stroke1),
			new StrokeRec(2, char90_stroke2)

		};
		// char: 91 '[' */

		static CoordRec[] char91_stroke0 = {
			new CoordRec(35.7143F, 119.048F),
			new CoordRec(35.7143F, -33.3333F)

		};
		static CoordRec[] char91_stroke1 = {
			new CoordRec(40.4762F, 119.048F),
			new CoordRec(40.4762F, -33.3333F)

		};
		static CoordRec[] char91_stroke2 = {
			new CoordRec(35.7143F, 119.048F),
			new CoordRec(69.0476F, 119.048F)

		};
		static CoordRec[] char91_stroke3 = {
			new CoordRec(35.7143F, -33.3333F),
			new CoordRec(69.0476F, -33.3333F)

		};
		static StrokeRec[] char91 = {
			new StrokeRec(2, char91_stroke0),
			new StrokeRec(2, char91_stroke1),
			new StrokeRec(2, char91_stroke2),
			new StrokeRec(2, char91_stroke3)

		};
		// char: 92 '\' */

		static CoordRec[] char92_stroke0 = {
			new CoordRec(19.0476F, 100F),
			new CoordRec(85.7143F, -14.2857F)

		};

		static StrokeRec[] char92 = { new StrokeRec(2, char92_stroke0) };
		// char: 93 ']' */

		static CoordRec[] char93_stroke0 = {
			new CoordRec(64.2857F, 119.048F),
			new CoordRec(64.2857F, -33.3333F)

		};
		static CoordRec[] char93_stroke1 = {
			new CoordRec(69.0476F, 119.048F),
			new CoordRec(69.0476F, -33.3333F)

		};
		static CoordRec[] char93_stroke2 = {
			new CoordRec(35.7143F, 119.048F),
			new CoordRec(69.0476F, 119.048F)

		};
		static CoordRec[] char93_stroke3 = {
			new CoordRec(35.7143F, -33.3333F),
			new CoordRec(69.0476F, -33.3333F)

		};
		static StrokeRec[] char93 = {
			new StrokeRec(2, char93_stroke0),
			new StrokeRec(2, char93_stroke1),
			new StrokeRec(2, char93_stroke2),
			new StrokeRec(2, char93_stroke3)

		};
		// char: 94 '^' */

		static CoordRec[] char94_stroke0 = {
			new CoordRec(52.3809F, 109.524F),
			new CoordRec(14.2857F, 42.8571F)

		};
		static CoordRec[] char94_stroke1 = {
			new CoordRec(52.3809F, 109.524F),
			new CoordRec(90.4762F, 42.8571F)

		};
		static StrokeRec[] char94 = {
			new StrokeRec(2, char94_stroke0),
			new StrokeRec(2, char94_stroke1)

		};
		// char: 95 '_' */

		static CoordRec[] char95_stroke0 = {
			new CoordRec(0F, -33.3333F),
			new CoordRec(104.762F, -33.3333F),
			new CoordRec(104.762F, -28.5714F),
			new CoordRec(0F, -28.5714F),
			new CoordRec(0F, -33.3333F)

		};

		static StrokeRec[] char95 = { new StrokeRec(5, char95_stroke0) };
		// char: 96 '`' */

		static CoordRec[] char96_stroke0 = {
			new CoordRec(42.8572F, 100F),
			new CoordRec(66.6667F, 71.4286F)

		};
		static CoordRec[] char96_stroke1 = {
			new CoordRec(42.8572F, 100F),
			new CoordRec(38.0953F, 95.2381F),
			new CoordRec(66.6667F, 71.4286F)

		};
		static StrokeRec[] char96 = {
			new StrokeRec(2, char96_stroke0),
			new StrokeRec(3, char96_stroke1)

		};
		// char: 97 'a' */

		static CoordRec[] char97_stroke0 = {
			new CoordRec(80.9524F, 66.6667F),
			new CoordRec(80.9524F, 0F)

		};
		static CoordRec[] char97_stroke1 = {
			new CoordRec(80.9524F, 52.381F),
			new CoordRec(71.4285F, 61.9048F),
			new CoordRec(61.9047F, 66.6667F),
			new CoordRec(47.619F, 66.6667F),
			new CoordRec(38.0952F, 61.9048F),
			new CoordRec(28.5714F, 52.381F),
			new CoordRec(23.8095F, 38.0952F),
			new CoordRec(23.8095F, 28.5714F),
			new CoordRec(28.5714F, 14.2857F),
			new CoordRec(38.0952F, 4.7619F),
			new CoordRec(47.619F, 0F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(71.4285F, 4.7619F),
			new CoordRec(80.9524F, 14.2857F)

		};
		static StrokeRec[] char97 = {
			new StrokeRec(2, char97_stroke0),
			new StrokeRec(14, char97_stroke1)

		};
		// char: 98 'b' */

		static CoordRec[] char98_stroke0 = {
			new CoordRec(23.8095F, 100F),
			new CoordRec(23.8095F, 0F)

		};
		static CoordRec[] char98_stroke1 = {
			new CoordRec(23.8095F, 52.381F),
			new CoordRec(33.3333F, 61.9048F),
			new CoordRec(42.8571F, 66.6667F),
			new CoordRec(57.1428F, 66.6667F),
			new CoordRec(66.6666F, 61.9048F),
			new CoordRec(76.1905F, 52.381F),
			new CoordRec(80.9524F, 38.0952F),
			new CoordRec(80.9524F, 28.5714F),
			new CoordRec(76.1905F, 14.2857F),
			new CoordRec(66.6666F, 4.7619F),
			new CoordRec(57.1428F, 0F),
			new CoordRec(42.8571F, 0F),
			new CoordRec(33.3333F, 4.7619F),
			new CoordRec(23.8095F, 14.2857F)

		};
		static StrokeRec[] char98 = {
			new StrokeRec(2, char98_stroke0),
			new StrokeRec(14, char98_stroke1)

		};
		// char: 99 'c' */

		static CoordRec[] char99_stroke0 = {
			new CoordRec(80.9524F, 52.381F),
			new CoordRec(71.4285F, 61.9048F),
			new CoordRec(61.9047F, 66.6667F),
			new CoordRec(47.619F, 66.6667F),
			new CoordRec(38.0952F, 61.9048F),
			new CoordRec(28.5714F, 52.381F),
			new CoordRec(23.8095F, 38.0952F),
			new CoordRec(23.8095F, 28.5714F),
			new CoordRec(28.5714F, 14.2857F),
			new CoordRec(38.0952F, 4.7619F),
			new CoordRec(47.619F, 0F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(71.4285F, 4.7619F),
			new CoordRec(80.9524F, 14.2857F)

		};

		static StrokeRec[] char99 = { new StrokeRec(14, char99_stroke0) };
		// char: 100 'd' */

		static CoordRec[] char100_stroke0 = {
			new CoordRec(80.9524F, 100F),
			new CoordRec(80.9524F, 0F)

		};
		static CoordRec[] char100_stroke1 = {
			new CoordRec(80.9524F, 52.381F),
			new CoordRec(71.4285F, 61.9048F),
			new CoordRec(61.9047F, 66.6667F),
			new CoordRec(47.619F, 66.6667F),
			new CoordRec(38.0952F, 61.9048F),
			new CoordRec(28.5714F, 52.381F),
			new CoordRec(23.8095F, 38.0952F),
			new CoordRec(23.8095F, 28.5714F),
			new CoordRec(28.5714F, 14.2857F),
			new CoordRec(38.0952F, 4.7619F),
			new CoordRec(47.619F, 0F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(71.4285F, 4.7619F),
			new CoordRec(80.9524F, 14.2857F)

		};
		static StrokeRec[] char100 = {
			new StrokeRec(2, char100_stroke0),
			new StrokeRec(14, char100_stroke1)

		};
		// char: 101 'e' */

		static CoordRec[] char101_stroke0 = {
			new CoordRec(23.8095F, 38.0952F),
			new CoordRec(80.9524F, 38.0952F),
			new CoordRec(80.9524F, 47.619F),
			new CoordRec(76.1905F, 57.1429F),
			new CoordRec(71.4285F, 61.9048F),
			new CoordRec(61.9047F, 66.6667F),
			new CoordRec(47.619F, 66.6667F),
			new CoordRec(38.0952F, 61.9048F),
			new CoordRec(28.5714F, 52.381F),
			new CoordRec(23.8095F, 38.0952F),
			new CoordRec(23.8095F, 28.5714F),
			new CoordRec(28.5714F, 14.2857F),
			new CoordRec(38.0952F, 4.7619F),
			new CoordRec(47.619F, 0F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(71.4285F, 4.7619F),
			new CoordRec(80.9524F, 14.2857F)

		};

		static StrokeRec[] char101 = { new StrokeRec(17, char101_stroke0) };
		// char: 102 'f' */

		static CoordRec[] char102_stroke0 = {
			new CoordRec(71.4286F, 100F),
			new CoordRec(61.9048F, 100F),
			new CoordRec(52.381F, 95.2381F),
			new CoordRec(47.6191F, 80.9524F),
			new CoordRec(47.6191F, 0F)

		};
		static CoordRec[] char102_stroke1 = {
			new CoordRec(33.3334F, 66.6667F),
			new CoordRec(66.6667F, 66.6667F)

		};
		static StrokeRec[] char102 = {
			new StrokeRec(5, char102_stroke0),
			new StrokeRec(2, char102_stroke1)

		};
		// char: 103 'g' */

		static CoordRec[] char103_stroke0 = {
			new CoordRec(80.9524F, 66.6667F),
			new CoordRec(80.9524F, -9.5238F),
			new CoordRec(76.1905F, -23.8095F),
			new CoordRec(71.4285F, -28.5714F),
			new CoordRec(61.9047F, -33.3333F),
			new CoordRec(47.619F, -33.3333F),
			new CoordRec(38.0952F, -28.5714F)

		};
		static CoordRec[] char103_stroke1 = {
			new CoordRec(80.9524F, 52.381F),
			new CoordRec(71.4285F, 61.9048F),
			new CoordRec(61.9047F, 66.6667F),
			new CoordRec(47.619F, 66.6667F),
			new CoordRec(38.0952F, 61.9048F),
			new CoordRec(28.5714F, 52.381F),
			new CoordRec(23.8095F, 38.0952F),
			new CoordRec(23.8095F, 28.5714F),
			new CoordRec(28.5714F, 14.2857F),
			new CoordRec(38.0952F, 4.7619F),
			new CoordRec(47.619F, 0F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(71.4285F, 4.7619F),
			new CoordRec(80.9524F, 14.2857F)

		};
		static StrokeRec[] char103 = {
			new StrokeRec(7, char103_stroke0),
			new StrokeRec(14, char103_stroke1)

		};
		// char: 104 'h' */

		static CoordRec[] char104_stroke0 = {
			new CoordRec(26.1905F, 100F),
			new CoordRec(26.1905F, 0F)

		};
		static CoordRec[] char104_stroke1 = {
			new CoordRec(26.1905F, 47.619F),
			new CoordRec(40.4762F, 61.9048F),
			new CoordRec(50F, 66.6667F),
			new CoordRec(64.2857F, 66.6667F),
			new CoordRec(73.8095F, 61.9048F),
			new CoordRec(78.5715F, 47.619F),
			new CoordRec(78.5715F, 0F)

		};
		static StrokeRec[] char104 = {
			new StrokeRec(2, char104_stroke0),
			new StrokeRec(7, char104_stroke1)

		};
		// char: 105 'i' */

		static CoordRec[] char105_stroke0 = {
			new CoordRec(47.6191F, 100F),
			new CoordRec(52.381F, 95.2381F),
			new CoordRec(57.1429F, 100F),
			new CoordRec(52.381F, 104.762F),
			new CoordRec(47.6191F, 100F)

		};
		static CoordRec[] char105_stroke1 = {
			new CoordRec(52.381F, 66.6667F),
			new CoordRec(52.381F, 0F)

		};
		static StrokeRec[] char105 = {
			new StrokeRec(5, char105_stroke0),
			new StrokeRec(2, char105_stroke1)

		};
		// char: 106 'j' */

		static CoordRec[] char106_stroke0 = {
			new CoordRec(57.1429F, 100F),
			new CoordRec(61.9048F, 95.2381F),
			new CoordRec(66.6667F, 100F),
			new CoordRec(61.9048F, 104.762F),
			new CoordRec(57.1429F, 100F)

		};
		static CoordRec[] char106_stroke1 = {
			new CoordRec(61.9048F, 66.6667F),
			new CoordRec(61.9048F, -14.2857F),
			new CoordRec(57.1429F, -28.5714F),
			new CoordRec(47.6191F, -33.3333F),
			new CoordRec(38.0953F, -33.3333F)

		};
		static StrokeRec[] char106 = {
			new StrokeRec(5, char106_stroke0),
			new StrokeRec(5, char106_stroke1)

		};
		// char: 107 'k' */

		static CoordRec[] char107_stroke0 = {
			new CoordRec(26.1905F, 100F),
			new CoordRec(26.1905F, 0F)

		};
		static CoordRec[] char107_stroke1 = {
			new CoordRec(73.8095F, 66.6667F),
			new CoordRec(26.1905F, 19.0476F)

		};
		static CoordRec[] char107_stroke2 = {
			new CoordRec(45.2381F, 38.0952F),
			new CoordRec(78.5715F, 0F)

		};
		static StrokeRec[] char107 = {
			new StrokeRec(2, char107_stroke0),
			new StrokeRec(2, char107_stroke1),
			new StrokeRec(2, char107_stroke2)

		};
		// char: 108 'l' */

		static CoordRec[] char108_stroke0 = {
			new CoordRec(52.381F, 100F),
			new CoordRec(52.381F, 0F)

		};

		static StrokeRec[] char108 = { new StrokeRec(2, char108_stroke0) };
		// char: 109 'm' */

		static CoordRec[] char109_stroke0 = {
			new CoordRec(0F, 66.6667F),
			new CoordRec(0F, 0F)

		};
		static CoordRec[] char109_stroke1 = {
			new CoordRec(0F, 47.619F),
			new CoordRec(14.2857F, 61.9048F),
			new CoordRec(23.8095F, 66.6667F),
			new CoordRec(38.0952F, 66.6667F),
			new CoordRec(47.619F, 61.9048F),
			new CoordRec(52.381F, 47.619F),
			new CoordRec(52.381F, 0F)

		};
		static CoordRec[] char109_stroke2 = {
			new CoordRec(52.381F, 47.619F),
			new CoordRec(66.6667F, 61.9048F),
			new CoordRec(76.1905F, 66.6667F),
			new CoordRec(90.4762F, 66.6667F),
			new CoordRec(100F, 61.9048F),
			new CoordRec(104.762F, 47.619F),
			new CoordRec(104.762F, 0F)

		};
		static StrokeRec[] char109 = {
			new StrokeRec(2, char109_stroke0),
			new StrokeRec(7, char109_stroke1),
			new StrokeRec(7, char109_stroke2)

		};
		// char: 110 'n' */

		static CoordRec[] char110_stroke0 = {
			new CoordRec(26.1905F, 66.6667F),
			new CoordRec(26.1905F, 0F)

		};
		static CoordRec[] char110_stroke1 = {
			new CoordRec(26.1905F, 47.619F),
			new CoordRec(40.4762F, 61.9048F),
			new CoordRec(50F, 66.6667F),
			new CoordRec(64.2857F, 66.6667F),
			new CoordRec(73.8095F, 61.9048F),
			new CoordRec(78.5715F, 47.619F),
			new CoordRec(78.5715F, 0F)

		};
		static StrokeRec[] char110 = {
			new StrokeRec(2, char110_stroke0),
			new StrokeRec(7, char110_stroke1)

		};
		// char: 111 'o' */

		static CoordRec[] char111_stroke0 = {
			new CoordRec(45.2381F, 66.6667F),
			new CoordRec(35.7143F, 61.9048F),
			new CoordRec(26.1905F, 52.381F),
			new CoordRec(21.4286F, 38.0952F),
			new CoordRec(21.4286F, 28.5714F),
			new CoordRec(26.1905F, 14.2857F),
			new CoordRec(35.7143F, 4.7619F),
			new CoordRec(45.2381F, 0F),
			new CoordRec(59.5238F, 0F),
			new CoordRec(69.0476F, 4.7619F),
			new CoordRec(78.5714F, 14.2857F),
			new CoordRec(83.3334F, 28.5714F),
			new CoordRec(83.3334F, 38.0952F),
			new CoordRec(78.5714F, 52.381F),
			new CoordRec(69.0476F, 61.9048F),
			new CoordRec(59.5238F, 66.6667F),
			new CoordRec(45.2381F, 66.6667F)

		};

		static StrokeRec[] char111 = { new StrokeRec(17, char111_stroke0) };
		// char: 112 'p' */

		static CoordRec[] char112_stroke0 = {
			new CoordRec(23.8095F, 66.6667F),
			new CoordRec(23.8095F, -33.3333F)

		};
		static CoordRec[] char112_stroke1 = {
			new CoordRec(23.8095F, 52.381F),
			new CoordRec(33.3333F, 61.9048F),
			new CoordRec(42.8571F, 66.6667F),
			new CoordRec(57.1428F, 66.6667F),
			new CoordRec(66.6666F, 61.9048F),
			new CoordRec(76.1905F, 52.381F),
			new CoordRec(80.9524F, 38.0952F),
			new CoordRec(80.9524F, 28.5714F),
			new CoordRec(76.1905F, 14.2857F),
			new CoordRec(66.6666F, 4.7619F),
			new CoordRec(57.1428F, 0F),
			new CoordRec(42.8571F, 0F),
			new CoordRec(33.3333F, 4.7619F),
			new CoordRec(23.8095F, 14.2857F)

		};
		static StrokeRec[] char112 = {
			new StrokeRec(2, char112_stroke0),
			new StrokeRec(14, char112_stroke1)

		};
		// char: 113 'q' */

		static CoordRec[] char113_stroke0 = {
			new CoordRec(80.9524F, 66.6667F),
			new CoordRec(80.9524F, -33.3333F)

		};
		static CoordRec[] char113_stroke1 = {
			new CoordRec(80.9524F, 52.381F),
			new CoordRec(71.4285F, 61.9048F),
			new CoordRec(61.9047F, 66.6667F),
			new CoordRec(47.619F, 66.6667F),
			new CoordRec(38.0952F, 61.9048F),
			new CoordRec(28.5714F, 52.381F),
			new CoordRec(23.8095F, 38.0952F),
			new CoordRec(23.8095F, 28.5714F),
			new CoordRec(28.5714F, 14.2857F),
			new CoordRec(38.0952F, 4.7619F),
			new CoordRec(47.619F, 0F),
			new CoordRec(61.9047F, 0F),
			new CoordRec(71.4285F, 4.7619F),
			new CoordRec(80.9524F, 14.2857F)

		};
		static StrokeRec[] char113 = {
			new StrokeRec(2, char113_stroke0),
			new StrokeRec(14, char113_stroke1)

		};
		// char: 114 'r' */

		static CoordRec[] char114_stroke0 = {
			new CoordRec(33.3334F, 66.6667F),
			new CoordRec(33.3334F, 0F)

		};
		static CoordRec[] char114_stroke1 = {
			new CoordRec(33.3334F, 38.0952F),
			new CoordRec(38.0953F, 52.381F),
			new CoordRec(47.6191F, 61.9048F),
			new CoordRec(57.1429F, 66.6667F),
			new CoordRec(71.4286F, 66.6667F)

		};
		static StrokeRec[] char114 = {
			new StrokeRec(2, char114_stroke0),
			new StrokeRec(5, char114_stroke1)

		};
		// char: 115 's' */

		static CoordRec[] char115_stroke0 = {
			new CoordRec(78.5715F, 52.381F),
			new CoordRec(73.8095F, 61.9048F),
			new CoordRec(59.5238F, 66.6667F),
			new CoordRec(45.2381F, 66.6667F),
			new CoordRec(30.9524F, 61.9048F),
			new CoordRec(26.1905F, 52.381F),
			new CoordRec(30.9524F, 42.8571F),
			new CoordRec(40.4762F, 38.0952F),
			new CoordRec(64.2857F, 33.3333F),
			new CoordRec(73.8095F, 28.5714F),
			new CoordRec(78.5715F, 19.0476F),
			new CoordRec(78.5715F, 14.2857F),
			new CoordRec(73.8095F, 4.7619F),
			new CoordRec(59.5238F, 0F),
			new CoordRec(45.2381F, 0F),
			new CoordRec(30.9524F, 4.7619F),
			new CoordRec(26.1905F, 14.2857F)

		};

		static StrokeRec[] char115 = { new StrokeRec(17, char115_stroke0) };
		// char: 116 't' */

		static CoordRec[] char116_stroke0 = {
			new CoordRec(47.6191F, 100F),
			new CoordRec(47.6191F, 19.0476F),
			new CoordRec(52.381F, 4.7619F),
			new CoordRec(61.9048F, 0F),
			new CoordRec(71.4286F, 0F)

		};
		static CoordRec[] char116_stroke1 = {
			new CoordRec(33.3334F, 66.6667F),
			new CoordRec(66.6667F, 66.6667F)

		};
		static StrokeRec[] char116 = {
			new StrokeRec(5, char116_stroke0),
			new StrokeRec(2, char116_stroke1)

		};
		// char: 117 'u' */

		static CoordRec[] char117_stroke0 = {
			new CoordRec(26.1905F, 66.6667F),
			new CoordRec(26.1905F, 19.0476F),
			new CoordRec(30.9524F, 4.7619F),
			new CoordRec(40.4762F, 0F),
			new CoordRec(54.7619F, 0F),
			new CoordRec(64.2857F, 4.7619F),
			new CoordRec(78.5715F, 19.0476F)

		};
		static CoordRec[] char117_stroke1 = {
			new CoordRec(78.5715F, 66.6667F),
			new CoordRec(78.5715F, 0F)

		};
		static StrokeRec[] char117 = {
			new StrokeRec(7, char117_stroke0),
			new StrokeRec(2, char117_stroke1)

		};
		// char: 118 'v' */

		static CoordRec[] char118_stroke0 = {
			new CoordRec(23.8095F, 66.6667F),
			new CoordRec(52.3809F, 0F)

		};
		static CoordRec[] char118_stroke1 = {
			new CoordRec(80.9524F, 66.6667F),
			new CoordRec(52.3809F, 0F)

		};
		static StrokeRec[] char118 = {
			new StrokeRec(2, char118_stroke0),
			new StrokeRec(2, char118_stroke1)

		};
		// char: 119 'w' */

		static CoordRec[] char119_stroke0 = {
			new CoordRec(14.2857F, 66.6667F),
			new CoordRec(33.3333F, 0F)

		};
		static CoordRec[] char119_stroke1 = {
			new CoordRec(52.3809F, 66.6667F),
			new CoordRec(33.3333F, 0F)

		};
		static CoordRec[] char119_stroke2 = {
			new CoordRec(52.3809F, 66.6667F),
			new CoordRec(71.4286F, 0F)

		};
		static CoordRec[] char119_stroke3 = {
			new CoordRec(90.4762F, 66.6667F),
			new CoordRec(71.4286F, 0F)

		};
		static StrokeRec[] char119 = {
			new StrokeRec(2, char119_stroke0),
			new StrokeRec(2, char119_stroke1),
			new StrokeRec(2, char119_stroke2),
			new StrokeRec(2, char119_stroke3)

		};
		// char: 120 'x' */

		static CoordRec[] char120_stroke0 = {
			new CoordRec(26.1905F, 66.6667F),
			new CoordRec(78.5715F, 0F)

		};
		static CoordRec[] char120_stroke1 = {
			new CoordRec(78.5715F, 66.6667F),
			new CoordRec(26.1905F, 0F)

		};
		static StrokeRec[] char120 = {
			new StrokeRec(2, char120_stroke0),
			new StrokeRec(2, char120_stroke1)

		};
		// char: 121 'y' */

		static CoordRec[] char121_stroke0 = {
			new CoordRec(26.1905F, 66.6667F),
			new CoordRec(54.7619F, 0F)

		};
		static CoordRec[] char121_stroke1 = {
			new CoordRec(83.3334F, 66.6667F),
			new CoordRec(54.7619F, 0F),
			new CoordRec(45.2381F, -19.0476F),
			new CoordRec(35.7143F, -28.5714F),
			new CoordRec(26.1905F, -33.3333F),
			new CoordRec(21.4286F, -33.3333F)

		};
		static StrokeRec[] char121 = {
			new StrokeRec(2, char121_stroke0),
			new StrokeRec(6, char121_stroke1)

		};
		// char: 122 'z' */

		static CoordRec[] char122_stroke0 = {
			new CoordRec(78.5715F, 66.6667F),
			new CoordRec(26.1905F, 0F)

		};
		static CoordRec[] char122_stroke1 = {
			new CoordRec(26.1905F, 66.6667F),
			new CoordRec(78.5715F, 66.6667F)

		};
		static CoordRec[] char122_stroke2 = {
			new CoordRec(26.1905F, 0F),
			new CoordRec(78.5715F, 0F)

		};
		static StrokeRec[] char122 = {
			new StrokeRec(2, char122_stroke0),
			new StrokeRec(2, char122_stroke1),
			new StrokeRec(2, char122_stroke2)

		};
		// char: 123 '{' */

		static CoordRec[] char123_stroke0 = {
			new CoordRec(64.2857F, 119.048F),
			new CoordRec(54.7619F, 114.286F),
			new CoordRec(50F, 109.524F),
			new CoordRec(45.2381F, 100F),
			new CoordRec(45.2381F, 90.4762F),
			new CoordRec(50F, 80.9524F),
			new CoordRec(54.7619F, 76.1905F),
			new CoordRec(59.5238F, 66.6667F),
			new CoordRec(59.5238F, 57.1429F),
			new CoordRec(50F, 47.619F)

		};
		static CoordRec[] char123_stroke1 = {
			new CoordRec(54.7619F, 114.286F),
			new CoordRec(50F, 104.762F),
			new CoordRec(50F, 95.2381F),
			new CoordRec(54.7619F, 85.7143F),
			new CoordRec(59.5238F, 80.9524F),
			new CoordRec(64.2857F, 71.4286F),
			new CoordRec(64.2857F, 61.9048F),
			new CoordRec(59.5238F, 52.381F),
			new CoordRec(40.4762F, 42.8571F),
			new CoordRec(59.5238F, 33.3333F),
			new CoordRec(64.2857F, 23.8095F),
			new CoordRec(64.2857F, 14.2857F),
			new CoordRec(59.5238F, 4.7619F),
			new CoordRec(54.7619F, 0F),
			new CoordRec(50F, -9.5238F),
			new CoordRec(50F, -19.0476F),
			new CoordRec(54.7619F, -28.5714F)

		};
		static CoordRec[] char123_stroke2 = {
			new CoordRec(50F, 38.0952F),
			new CoordRec(59.5238F, 28.5714F),
			new CoordRec(59.5238F, 19.0476F),
			new CoordRec(54.7619F, 9.5238F),
			new CoordRec(50F, 4.7619F),
			new CoordRec(45.2381F, -4.7619F),
			new CoordRec(45.2381F, -14.2857F),
			new CoordRec(50F, -23.8095F),
			new CoordRec(54.7619F, -28.5714F),
			new CoordRec(64.2857F, -33.3333F)

		};
		static StrokeRec[] char123 = {
			new StrokeRec(10, char123_stroke0),
			new StrokeRec(17, char123_stroke1),
			new StrokeRec(10, char123_stroke2)

		};
		// char: 124 '|' */

		static CoordRec[] char124_stroke0 = {
			new CoordRec(52.381F, 119.048F),
			new CoordRec(52.381F, -33.3333F)

		};

		static StrokeRec[] char124 = { new StrokeRec(2, char124_stroke0) };
		// char: 125 '}' */

		static CoordRec[] char125_stroke0 = {
			new CoordRec(40.4762F, 119.048F),
			new CoordRec(50F, 114.286F),
			new CoordRec(54.7619F, 109.524F),
			new CoordRec(59.5238F, 100F),
			new CoordRec(59.5238F, 90.4762F),
			new CoordRec(54.7619F, 80.9524F),
			new CoordRec(50F, 76.1905F),
			new CoordRec(45.2381F, 66.6667F),
			new CoordRec(45.2381F, 57.1429F),
			new CoordRec(54.7619F, 47.619F)

		};
		static CoordRec[] char125_stroke1 = {
			new CoordRec(50F, 114.286F),
			new CoordRec(54.7619F, 104.762F),
			new CoordRec(54.7619F, 95.2381F),
			new CoordRec(50F, 85.7143F),
			new CoordRec(45.2381F, 80.9524F),
			new CoordRec(40.4762F, 71.4286F),
			new CoordRec(40.4762F, 61.9048F),
			new CoordRec(45.2381F, 52.381F),
			new CoordRec(64.2857F, 42.8571F),
			new CoordRec(45.2381F, 33.3333F),
			new CoordRec(40.4762F, 23.8095F),
			new CoordRec(40.4762F, 14.2857F),
			new CoordRec(45.2381F, 4.7619F),
			new CoordRec(50F, 0F),
			new CoordRec(54.7619F, -9.5238F),
			new CoordRec(54.7619F, -19.0476F),
			new CoordRec(50F, -28.5714F)

		};
		static CoordRec[] char125_stroke2 = {
			new CoordRec(54.7619F, 38.0952F),
			new CoordRec(45.2381F, 28.5714F),
			new CoordRec(45.2381F, 19.0476F),
			new CoordRec(50F, 9.5238F),
			new CoordRec(54.7619F, 4.7619F),
			new CoordRec(59.5238F, -4.7619F),
			new CoordRec(59.5238F, -14.2857F),
			new CoordRec(54.7619F, -23.8095F),
			new CoordRec(50F, -28.5714F),
			new CoordRec(40.4762F, -33.3333F)

		};
		static StrokeRec[] char125 = {
			new StrokeRec(10, char125_stroke0),
			new StrokeRec(17, char125_stroke1),
			new StrokeRec(10, char125_stroke2)

		};
		// char: 126 '~' */

		static CoordRec[] char126_stroke0 = {
			new CoordRec(9.5238F, 28.5714F),
			new CoordRec(9.5238F, 38.0952F),
			new CoordRec(14.2857F, 52.381F),
			new CoordRec(23.8095F, 57.1429F),
			new CoordRec(33.3333F, 57.1429F),
			new CoordRec(42.8571F, 52.381F),
			new CoordRec(61.9048F, 38.0952F),
			new CoordRec(71.4286F, 33.3333F),
			new CoordRec(80.9524F, 33.3333F),
			new CoordRec(90.4762F, 38.0952F),
			new CoordRec(95.2381F, 47.619F)

		};
		static CoordRec[] char126_stroke1 = {
			new CoordRec(9.5238F, 38.0952F),
			new CoordRec(14.2857F, 47.619F),
			new CoordRec(23.8095F, 52.381F),
			new CoordRec(33.3333F, 52.381F),
			new CoordRec(42.8571F, 47.619F),
			new CoordRec(61.9048F, 33.3333F),
			new CoordRec(71.4286F, 28.5714F),
			new CoordRec(80.9524F, 28.5714F),
			new CoordRec(90.4762F, 33.3333F),
			new CoordRec(95.2381F, 47.619F),
			new CoordRec(95.2381F, 57.1429F)

		};
		static StrokeRec[] char126 = {
			new StrokeRec(11, char126_stroke0),
			new StrokeRec(11, char126_stroke1)

		};
		// char: 127 */

		static CoordRec[] char127_stroke0 = {
			new CoordRec(71.4286F, 100F),
			new CoordRec(33.3333F, -33.3333F)

		};
		static CoordRec[] char127_stroke1 = {
			new CoordRec(47.619F, 66.6667F),
			new CoordRec(33.3333F, 61.9048F),
			new CoordRec(23.8095F, 52.381F),
			new CoordRec(19.0476F, 38.0952F),
			new CoordRec(19.0476F, 23.8095F),
			new CoordRec(23.8095F, 14.2857F),
			new CoordRec(33.3333F, 4.7619F),
			new CoordRec(47.619F, 0F),
			new CoordRec(57.1428F, 0F),
			new CoordRec(71.4286F, 4.7619F),
			new CoordRec(80.9524F, 14.2857F),
			new CoordRec(85.7143F, 28.5714F),
			new CoordRec(85.7143F, 42.8571F),
			new CoordRec(80.9524F, 52.381F),
			new CoordRec(71.4286F, 61.9048F),
			new CoordRec(57.1428F, 66.6667F),
			new CoordRec(47.619F, 66.6667F)

		};
		static StrokeRec[] char127 = {
			new StrokeRec(2, char127_stroke0),
			new StrokeRec(17, char127_stroke1)

		};
		static StrokeCharRec[] chars = {
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 0f, 0f),
			new StrokeCharRec(0, null, 52.381f, 104.762f),
			new StrokeCharRec(2, char33, 52.381f, 104.762f),
			new StrokeCharRec(2, char34, 52.381f, 104.762f),
			new StrokeCharRec(4, char35, 52.381f, 104.762f),
			new StrokeCharRec(3, char36, 52.381f, 104.762f),
			new StrokeCharRec(3, char37, 52.381f, 104.762f),
			new StrokeCharRec(1, char38, 52.381f, 104.762f),
			new StrokeCharRec(1, char39, 52.381f, 104.762f),
			new StrokeCharRec(1, char40, 52.381f, 104.762f),
			new StrokeCharRec(1, char41, 52.381f, 104.762f),
			new StrokeCharRec(3, char42, 52.381f, 104.762f),
			new StrokeCharRec(2, char43, 52.381f, 104.762f),
			new StrokeCharRec(1, char44, 52.381f, 104.762f),
			new StrokeCharRec(1, char45, 52.381f, 104.762f),
			new StrokeCharRec(1, char46, 52.381f, 104.762f),
			new StrokeCharRec(1, char47, 52.381f, 104.762f),
			new StrokeCharRec(1, char48, 52.381f, 104.762f),
			new StrokeCharRec(1, char49, 52.381f, 104.762f),
			new StrokeCharRec(1, char50, 52.381f, 104.762f),
			new StrokeCharRec(1, char51, 52.381f, 104.762f),
			new StrokeCharRec(2, char52, 52.381f, 104.762f),
			new StrokeCharRec(1, char53, 52.381f, 104.762f),
			new StrokeCharRec(1, char54, 52.381f, 104.762f),
			new StrokeCharRec(2, char55, 52.381f, 104.762f),
			new StrokeCharRec(1, char56, 52.381f, 104.762f),
			new StrokeCharRec(1, char57, 52.381f, 104.762f),
			new StrokeCharRec(2, char58, 52.381f, 104.762f),
			new StrokeCharRec(2, char59, 52.381f, 104.762f),
			new StrokeCharRec(1, char60, 52.381f, 104.762f),
			new StrokeCharRec(2, char61, 52.381f, 104.762f),
			new StrokeCharRec(1, char62, 52.381f, 104.762f),
			new StrokeCharRec(2, char63, 52.381f, 104.762f),
			new StrokeCharRec(2, char64, 52.381f, 104.762f),
			new StrokeCharRec(3, char65, 52.381f, 104.762f),
			new StrokeCharRec(3, char66, 52.381f, 104.762f),
			new StrokeCharRec(1, char67, 52.381f, 104.762f),
			new StrokeCharRec(2, char68, 52.381f, 104.762f),
			new StrokeCharRec(4, char69, 52.381f, 104.762f),
			new StrokeCharRec(3, char70, 52.381f, 104.762f),
			new StrokeCharRec(2, char71, 52.381f, 104.762f),
			new StrokeCharRec(3, char72, 52.381f, 104.762f),
			new StrokeCharRec(1, char73, 52.381f, 104.762f),
			new StrokeCharRec(1, char74, 52.381f, 104.762f),
			new StrokeCharRec(3, char75, 52.381f, 104.762f),
			new StrokeCharRec(2, char76, 52.381f, 104.762f),
			new StrokeCharRec(4, char77, 52.381f, 104.762f),
			new StrokeCharRec(3, char78, 52.381f, 104.762f),
			new StrokeCharRec(1, char79, 52.381f, 104.762f),
			new StrokeCharRec(2, char80, 52.381f, 104.762f),
			new StrokeCharRec(2, char81, 52.381f, 104.762f),
			new StrokeCharRec(3, char82, 52.381f, 104.762f),
			new StrokeCharRec(1, char83, 52.381f, 104.762f),
			new StrokeCharRec(2, char84, 52.381f, 104.762f),
			new StrokeCharRec(1, char85, 52.381f, 104.762f),
			new StrokeCharRec(2, char86, 52.381f, 104.762f),
			new StrokeCharRec(4, char87, 52.381f, 104.762f),
			new StrokeCharRec(2, char88, 52.381f, 104.762f),
			new StrokeCharRec(2, char89, 52.381f, 104.762f),
			new StrokeCharRec(3, char90, 52.381f, 104.762f),
			new StrokeCharRec(4, char91, 52.381f, 104.762f),
			new StrokeCharRec(1, char92, 52.381f, 104.762f),
			new StrokeCharRec(4, char93, 52.381f, 104.762f),
			new StrokeCharRec(2, char94, 52.381f, 104.762f),
			new StrokeCharRec(1, char95, 52.381f, 104.762f),
			new StrokeCharRec(2, char96, 52.381f, 104.762f),
			new StrokeCharRec(2, char97, 52.381f, 104.762f),
			new StrokeCharRec(2, char98, 52.381f, 104.762f),
			new StrokeCharRec(1, char99, 52.381f, 104.762f),
			new StrokeCharRec(2, char100, 52.381f, 104.762f),
			new StrokeCharRec(1, char101, 52.381f, 104.762f),
			new StrokeCharRec(2, char102, 52.381f, 104.762f),
			new StrokeCharRec(2, char103, 52.381f, 104.762f),
			new StrokeCharRec(2, char104, 52.381f, 104.762f),
			new StrokeCharRec(2, char105, 52.381f, 104.762f),
			new StrokeCharRec(2, char106, 52.381f, 104.762f),
			new StrokeCharRec(3, char107, 52.381f, 104.762f),
			new StrokeCharRec(1, char108, 52.381f, 104.762f),
			new StrokeCharRec(3, char109, 52.381f, 104.762f),
			new StrokeCharRec(2, char110, 52.381f, 104.762f),
			new StrokeCharRec(1, char111, 52.381f, 104.762f),
			new StrokeCharRec(2, char112, 52.381f, 104.762f),
			new StrokeCharRec(2, char113, 52.381f, 104.762f),
			new StrokeCharRec(2, char114, 52.381f, 104.762f),
			new StrokeCharRec(1, char115, 52.381f, 104.762f),
			new StrokeCharRec(2, char116, 52.381f, 104.762f),
			new StrokeCharRec(2, char117, 52.381f, 104.762f),
			new StrokeCharRec(2, char118, 52.381f, 104.762f),
			new StrokeCharRec(4, char119, 52.381f, 104.762f),
			new StrokeCharRec(2, char120, 52.381f, 104.762f),
			new StrokeCharRec(2, char121, 52.381f, 104.762f),
			new StrokeCharRec(3, char122, 52.381f, 104.762f),
			new StrokeCharRec(3, char123, 52.381f, 104.762f),
			new StrokeCharRec(1, char124, 52.381f, 104.762f),
			new StrokeCharRec(3, char125, 52.381f, 104.762f),
			new StrokeCharRec(2, char126, 52.381f, 104.762f),
			new StrokeCharRec(2, char127, 52.381f, 104.762f)

		};
		public static StrokeFontRec glutStrokeMonoRoman = new StrokeFontRec("Roman", 128, chars, 119.048f, -33.3333f);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
