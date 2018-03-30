
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace nzy3D.Glut
{

	public class GLUTBitmap9x15
	{


		static BitmapCharRec ch0 = new BitmapCharRec(0, 0, 0, 0, 9, null);

		static BitmapCharRec ch32 = new BitmapCharRec(0, 0, 0, 0, 9, null);

		static BitmapCharRec ch127 = new BitmapCharRec(0, 0, 0, 0, 9, null);

		static BitmapCharRec ch160 = new BitmapCharRec(0, 0, 0, 0, 9, null);
		//char: &Hff */

		static byte[] ch255data = {
			0x78,
			0x84,
			0x4,
			0x74,
			0x8c,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84,
			0x0,
			0x0,
			0x28,
			0x28

		};



		static BitmapCharRec ch255 = new BitmapCharRec(6, 14, -1, 3, 9, ch255data);
		//char: &Hfe */

		static byte[] ch254data = {
			0x80,
			0x80,
			0x80,
			0xbc,
			0xc2,
			0x82,
			0x82,
			0x82,
			0xc2,
			0xbc,
			0x80,
			0x80

		};

		static BitmapCharRec ch254 = new BitmapCharRec(7, 12, -1, 3, 9, ch254data);
		//char: &Hfd */

		static byte[] ch253data = {
			0x78,
			0x84,
			0x4,
			0x74,
			0x8c,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84,
			0x0,
			0x0,
			0x30,
			0x8

		};

		static BitmapCharRec ch253 = new BitmapCharRec(6, 14, -1, 3, 9, ch253data);
		//char: &Hfc */

		static byte[] ch252data = {
			0x7a,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84,
			0x0,
			0x0,
			0x28,
			0x28

		};

		static BitmapCharRec ch252 = new BitmapCharRec(7, 11, -1, 0, 9, ch252data);
		//char: &Hfb */

		static byte[] ch251data = {
			0x7a,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84,
			0x0,
			0x0,
			0x44,
			0x38

		};

		static BitmapCharRec ch251 = new BitmapCharRec(7, 11, -1, 0, 9, ch251data);
		//char: &Hfa */

		static byte[] ch250data = {
			0x7a,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84,
			0x0,
			0x0,
			0x30,
			0x8

		};

		static BitmapCharRec ch250 = new BitmapCharRec(7, 11, -1, 0, 9, ch250data);
		//char: &Hf9 */

		static byte[] ch249data = {
			0x7a,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84,
			0x0,
			0x0,
			0x18,
			0x20

		};

		static BitmapCharRec ch249 = new BitmapCharRec(7, 11, -1, 0, 9, ch249data);
		//char: &Hf8 */

		static byte[] ch248data = {
			0x80,
			0x7c,
			0xa2,
			0xa2,
			0x92,
			0x8a,
			0x8a,
			0x7c,
			0x2

		};

		static BitmapCharRec ch248 = new BitmapCharRec(7, 9, -1, 1, 9, ch248data);
		//char: &Hf7 */

		static byte[] ch247data = {
			0x10,
			0x38,
			0x10,
			0x0,
			0xfe,
			0x0,
			0x10,
			0x38,
			0x10

		};

		static BitmapCharRec ch247 = new BitmapCharRec(7, 9, -1, 0, 9, ch247data);
		//char: &Hf6 */

		static byte[] ch246data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x0,
			0x28,
			0x28

		};

		static BitmapCharRec ch246 = new BitmapCharRec(7, 11, -1, 0, 9, ch246data);
		//char: &Hf5 */

		static byte[] ch245data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x0,
			0x50,
			0x28

		};

		static BitmapCharRec ch245 = new BitmapCharRec(7, 11, -1, 0, 9, ch245data);
		//char: &Hf4 */

		static byte[] ch244data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x0,
			0x44,
			0x38

		};

		static BitmapCharRec ch244 = new BitmapCharRec(7, 11, -1, 0, 9, ch244data);
		//char: &Hf3 */

		static byte[] ch243data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x0,
			0x30,
			0x8

		};

		static BitmapCharRec ch243 = new BitmapCharRec(7, 11, -1, 0, 9, ch243data);
		//char: &Hf2 */

		static byte[] ch242data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x0,
			0x18,
			0x20

		};

		static BitmapCharRec ch242 = new BitmapCharRec(7, 11, -1, 0, 9, ch242data);
		//char: &Hf1 */

		static byte[] ch241data = {
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0xc2,
			0xbc,
			0x0,
			0x0,
			0x50,
			0x28

		};

		static BitmapCharRec ch241 = new BitmapCharRec(7, 11, -1, 0, 9, ch241data);
		//char: &Hf0 */

		static byte[] ch240data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c,
			0x8,
			0x50,
			0x30,
			0x48

		};

		static BitmapCharRec ch240 = new BitmapCharRec(7, 11, -1, 0, 9, ch240data);
		//char: &Hef */

		static byte[] ch239data = {
			0xf8,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0xe0,
			0x0,
			0x0,
			0x50,
			0x50

		};

		static BitmapCharRec ch239 = new BitmapCharRec(5, 11, -2, 0, 9, ch239data);
		//char: &Hee */

		static byte[] ch238data = {
			0xf8,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0xe0,
			0x0,
			0x0,
			0x90,
			0x60

		};

		static BitmapCharRec ch238 = new BitmapCharRec(5, 11, -2, 0, 9, ch238data);
		//char: &Hed */

		static byte[] ch237data = {
			0xf8,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0xe0,
			0x0,
			0x0,
			0x60,
			0x10

		};

		static BitmapCharRec ch237 = new BitmapCharRec(5, 11, -2, 0, 9, ch237data);
		//char: &Hec */

		static byte[] ch236data = {
			0xf8,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0xe0,
			0x0,
			0x0,
			0x30,
			0x40

		};

		static BitmapCharRec ch236 = new BitmapCharRec(5, 11, -2, 0, 9, ch236data);
		//char: &Heb */

		static byte[] ch235data = {
			0x7c,
			0x80,
			0x80,
			0xfe,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x0,
			0x28,
			0x28

		};

		static BitmapCharRec ch235 = new BitmapCharRec(7, 11, -1, 0, 9, ch235data);
		//char: &Hea */

		static byte[] ch234data = {
			0x7c,
			0x80,
			0x80,
			0xfe,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x0,
			0x44,
			0x38

		};

		static BitmapCharRec ch234 = new BitmapCharRec(7, 11, -1, 0, 9, ch234data);
		//char: &He9 */

		static byte[] ch233data = {
			0x7c,
			0x80,
			0x80,
			0xfe,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x0,
			0x30,
			0x8

		};

		static BitmapCharRec ch233 = new BitmapCharRec(7, 11, -1, 0, 9, ch233data);
		//char: &He8 */

		static byte[] ch232data = {
			0x7c,
			0x80,
			0x80,
			0xfe,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x0,
			0x18,
			0x20

		};

		static BitmapCharRec ch232 = new BitmapCharRec(7, 11, -1, 0, 9, ch232data);
		//char: &He7 */

		static byte[] ch231data = {
			0x30,
			0x48,
			0x18,
			0x7c,
			0x82,
			0x80,
			0x80,
			0x80,
			0x82,
			0x7c

		};

		static BitmapCharRec ch231 = new BitmapCharRec(7, 10, -1, 3, 9, ch231data);
		//char: &He6 */

		static byte[] ch230data = {
			0x6e,
			0x92,
			0x90,
			0x7c,
			0x12,
			0x92,
			0x6c

		};

		static BitmapCharRec ch230 = new BitmapCharRec(7, 7, -1, 0, 9, ch230data);
		//char: &He5 */

		static byte[] ch229data = {
			0x7a,
			0x86,
			0x82,
			0x7e,
			0x2,
			0x2,
			0x7c,
			0x0,
			0x18,
			0x24,
			0x18

		};

		static BitmapCharRec ch229 = new BitmapCharRec(7, 11, -1, 0, 9, ch229data);
		//char: &He4 */

		static byte[] ch228data = {
			0x7a,
			0x86,
			0x82,
			0x7e,
			0x2,
			0x2,
			0x7c,
			0x0,
			0x0,
			0x28,
			0x28

		};

		static BitmapCharRec ch228 = new BitmapCharRec(7, 11, -1, 0, 9, ch228data);
		//char: &He3 */

		static byte[] ch227data = {
			0x7a,
			0x86,
			0x82,
			0x7e,
			0x2,
			0x2,
			0x7c,
			0x0,
			0x0,
			0x50,
			0x28

		};

		static BitmapCharRec ch227 = new BitmapCharRec(7, 11, -1, 0, 9, ch227data);
		//char: &He2 */

		static byte[] ch226data = {
			0x7a,
			0x86,
			0x82,
			0x7e,
			0x2,
			0x2,
			0x7c,
			0x0,
			0x0,
			0x44,
			0x38

		};

		static BitmapCharRec ch226 = new BitmapCharRec(7, 11, -1, 0, 9, ch226data);
		//char: &He1 */

		static byte[] ch225data = {
			0x7a,
			0x86,
			0x82,
			0x7e,
			0x2,
			0x2,
			0x7c,
			0x0,
			0x0,
			0x30,
			0x8

		};

		static BitmapCharRec ch225 = new BitmapCharRec(7, 11, -1, 0, 9, ch225data);
		//char: &He0 */

		static byte[] ch224data = {
			0x7a,
			0x86,
			0x82,
			0x7e,
			0x2,
			0x2,
			0x7c,
			0x0,
			0x0,
			0x18,
			0x20

		};

		static BitmapCharRec ch224 = new BitmapCharRec(7, 11, -1, 0, 9, ch224data);
		//char: &Hdf */

		static byte[] ch223data = {
			0x80,
			0xbc,
			0xc2,
			0x82,
			0x82,
			0xfc,
			0x82,
			0x82,
			0x7c

		};

		static BitmapCharRec ch223 = new BitmapCharRec(7, 9, -1, 1, 9, ch223data);
		//char: &Hde */

		static byte[] ch222data = {
			0x80,
			0x80,
			0x80,
			0xfc,
			0x82,
			0x82,
			0x82,
			0xfc,
			0x80,
			0x80

		};

		static BitmapCharRec ch222 = new BitmapCharRec(7, 10, -1, 0, 9, ch222data);
		//char: &Hdd */

		static byte[] ch221data = {
			0x10,
			0x10,
			0x10,
			0x10,
			0x28,
			0x44,
			0x82,
			0x82,
			0x0,
			0x30,
			0x8

		};

		static BitmapCharRec ch221 = new BitmapCharRec(7, 11, -1, 0, 9, ch221data);
		//char: &Hdc */

		static byte[] ch220data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x0,
			0x28,
			0x28

		};

		static BitmapCharRec ch220 = new BitmapCharRec(7, 11, -1, 0, 9, ch220data);
		//char: &Hdb */

		static byte[] ch219data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x0,
			0x44,
			0x38

		};

		static BitmapCharRec ch219 = new BitmapCharRec(7, 11, -1, 0, 9, ch219data);
		//char: &Hda */

		static byte[] ch218data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x0,
			0x30,
			0x8

		};

		static BitmapCharRec ch218 = new BitmapCharRec(7, 11, -1, 0, 9, ch218data);
		//char: &Hd9 */

		static byte[] ch217data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x0,
			0x18,
			0x20

		};

		static BitmapCharRec ch217 = new BitmapCharRec(7, 11, -1, 0, 9, ch217data);
		//char: &Hd8 */

		static byte[] ch216data = {
			0x80,
			0x7c,
			0xc2,
			0xa2,
			0xa2,
			0x92,
			0x92,
			0x8a,
			0x8a,
			0x86,
			0x7c,
			0x2

		};

		static BitmapCharRec ch216 = new BitmapCharRec(7, 12, -1, 1, 9, ch216data);
		//char: &Hd7 */

		static byte[] ch215data = {
			0x82,
			0x44,
			0x28,
			0x10,
			0x28,
			0x44,
			0x82

		};

		static BitmapCharRec ch215 = new BitmapCharRec(7, 7, -1, -1, 9, ch215data);
		//char: &Hd6 */

		static byte[] ch214data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x28,
			0x28

		};

		static BitmapCharRec ch214 = new BitmapCharRec(7, 11, -1, 0, 9, ch214data);
		//char: &Hd5 */

		static byte[] ch213data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x50,
			0x28

		};

		static BitmapCharRec ch213 = new BitmapCharRec(7, 11, -1, 0, 9, ch213data);
		//char: &Hd4 */

		static byte[] ch212data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x44,
			0x38

		};

		static BitmapCharRec ch212 = new BitmapCharRec(7, 11, -1, 0, 9, ch212data);
		//char: &Hd3 */

		static byte[] ch211data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x30,
			0x8

		};

		static BitmapCharRec ch211 = new BitmapCharRec(7, 11, -1, 0, 9, ch211data);
		//char: &Hd2 */

		static byte[] ch210data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c,
			0x0,
			0x18,
			0x20

		};

		static BitmapCharRec ch210 = new BitmapCharRec(7, 11, -1, 0, 9, ch210data);
		//char: &Hd1 */

		static byte[] ch209data = {
			0x82,
			0x86,
			0x8a,
			0x92,
			0x92,
			0xa2,
			0xc2,
			0x82,
			0x0,
			0x50,
			0x28

		};

		static BitmapCharRec ch209 = new BitmapCharRec(7, 11, -1, 0, 9, ch209data);
		//char: &Hd0 */

		static byte[] ch208data = {
			0xfc,
			0x42,
			0x42,
			0x42,
			0x42,
			0xf2,
			0x42,
			0x42,
			0x42,
			0xfc

		};

		static BitmapCharRec ch208 = new BitmapCharRec(7, 10, -1, 0, 9, ch208data);
		//char: &Hcf */

		static byte[] ch207data = {
			0xf8,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0xf8,
			0x0,
			0x50,
			0x50

		};

		static BitmapCharRec ch207 = new BitmapCharRec(5, 11, -2, 0, 9, ch207data);
		//char: &Hce */

		static byte[] ch206data = {
			0xf8,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0xf8,
			0x0,
			0x88,
			0x70

		};

		static BitmapCharRec ch206 = new BitmapCharRec(5, 11, -2, 0, 9, ch206data);
		//char: &Hcd */

		static byte[] ch205data = {
			0xf8,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0xf8,
			0x0,
			0x60,
			0x10

		};

		static BitmapCharRec ch205 = new BitmapCharRec(5, 11, -2, 0, 9, ch205data);
		//char: &Hcc */

		static byte[] ch204data = {
			0xf8,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0xf8,
			0x0,
			0x30,
			0x40

		};

		static BitmapCharRec ch204 = new BitmapCharRec(5, 11, -2, 0, 9, ch204data);
		//char: &Hcb */

		static byte[] ch203data = {
			0xfe,
			0x40,
			0x40,
			0x40,
			0x78,
			0x40,
			0x40,
			0xfe,
			0x0,
			0x28,
			0x28

		};

		static BitmapCharRec ch203 = new BitmapCharRec(7, 11, -1, 0, 9, ch203data);
		//char: &Hca */

		static byte[] ch202data = {
			0xfe,
			0x40,
			0x40,
			0x40,
			0x78,
			0x40,
			0x40,
			0xfe,
			0x0,
			0x44,
			0x38

		};

		static BitmapCharRec ch202 = new BitmapCharRec(7, 11, -1, 0, 9, ch202data);
		//char: &Hc9 */

		static byte[] ch201data = {
			0xfe,
			0x40,
			0x40,
			0x40,
			0x78,
			0x40,
			0x40,
			0xfe,
			0x0,
			0x30,
			0x8

		};

		static BitmapCharRec ch201 = new BitmapCharRec(7, 11, -1, 0, 9, ch201data);
		//char: &Hc8 */

		static byte[] ch200data = {
			0xfe,
			0x40,
			0x40,
			0x40,
			0x78,
			0x40,
			0x40,
			0xfe,
			0x0,
			0x18,
			0x20

		};

		static BitmapCharRec ch200 = new BitmapCharRec(7, 11, -1, 0, 9, ch200data);
		//char: &Hc7 */

		static byte[] ch199data = {
			0x30,
			0x48,
			0x18,
			0x7c,
			0x82,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x82,
			0x7c

		};

		static BitmapCharRec ch199 = new BitmapCharRec(7, 13, -1, 3, 9, ch199data);
		//char: &Hc6 */

		static byte[] ch198data = {
			0x9e,
			0x90,
			0x90,
			0x90,
			0xfc,
			0x90,
			0x90,
			0x90,
			0x90,
			0x6e

		};

		static BitmapCharRec ch198 = new BitmapCharRec(7, 10, -1, 0, 9, ch198data);
		//char: &Hc5 */

		static byte[] ch197data = {
			0x82,
			0x82,
			0x82,
			0xfe,
			0x82,
			0x82,
			0x44,
			0x38,
			0x10,
			0x28,
			0x10

		};

		static BitmapCharRec ch197 = new BitmapCharRec(7, 11, -1, 0, 9, ch197data);
		//char: &Hc4 */

		static byte[] ch196data = {
			0x82,
			0x82,
			0x82,
			0xfe,
			0x82,
			0x82,
			0x44,
			0x38,
			0x0,
			0x28,
			0x28

		};

		static BitmapCharRec ch196 = new BitmapCharRec(7, 11, -1, 0, 9, ch196data);
		//char: &Hc3 */

		static byte[] ch195data = {
			0x82,
			0x82,
			0x82,
			0xfe,
			0x82,
			0x82,
			0x44,
			0x38,
			0x0,
			0x50,
			0x28

		};

		static BitmapCharRec ch195 = new BitmapCharRec(7, 11, -1, 0, 9, ch195data);
		//char: &Hc2 */

		static byte[] ch194data = {
			0x82,
			0x82,
			0x82,
			0xfe,
			0x82,
			0x82,
			0x44,
			0x38,
			0x0,
			0x44,
			0x38

		};

		static BitmapCharRec ch194 = new BitmapCharRec(7, 11, -1, 0, 9, ch194data);
		//char: &Hc1 */

		static byte[] ch193data = {
			0x82,
			0x82,
			0x82,
			0xfe,
			0x82,
			0x82,
			0x44,
			0x38,
			0x0,
			0x30,
			0x8

		};

		static BitmapCharRec ch193 = new BitmapCharRec(7, 11, -1, 0, 9, ch193data);
		//char: &Hc0 */

		static byte[] ch192data = {
			0x82,
			0x82,
			0x82,
			0xfe,
			0x82,
			0x82,
			0x44,
			0x38,
			0x0,
			0x18,
			0x20

		};

		static BitmapCharRec ch192 = new BitmapCharRec(7, 11, -1, 0, 9, ch192data);
		//char: &Hbf */

		static byte[] ch191data = {
			0x7c,
			0x82,
			0x82,
			0x80,
			0x40,
			0x20,
			0x10,
			0x10,
			0x0,
			0x10

		};

		static BitmapCharRec ch191 = new BitmapCharRec(7, 10, -1, 0, 9, ch191data);
		//char: &Hbe */

		static byte[] ch190data = {
			0x6,
			0x1a,
			0x12,
			0xa,
			0x66,
			0x92,
			0x10,
			0x20,
			0x90,
			0x60

		};

		static BitmapCharRec ch190 = new BitmapCharRec(7, 10, -1, 0, 9, ch190data);
		//char: &Hbd */

		static byte[] ch189data = {
			0x1e,
			0x10,
			0xc,
			0x2,
			0xf2,
			0x4c,
			0x40,
			0x40,
			0xc0,
			0x40

		};

		static BitmapCharRec ch189 = new BitmapCharRec(7, 10, -1, 0, 9, ch189data);
		//char: &Hbc */

		static byte[] ch188data = {
			0x6,
			0x1a,
			0x12,
			0xa,
			0xe6,
			0x42,
			0x40,
			0x40,
			0xc0,
			0x40

		};

		static BitmapCharRec ch188 = new BitmapCharRec(7, 10, -1, 0, 9, ch188data);
		//char: &Hbb */

		static byte[] ch187data = {
			0x90,
			0x48,
			0x24,
			0x12,
			0x12,
			0x24,
			0x48,
			0x90

		};

		static BitmapCharRec ch187 = new BitmapCharRec(7, 8, -1, -1, 9, ch187data);
		//char: &Hba */

		static byte[] ch186data = {
			0xf8,
			0x0,
			0x70,
			0x88,
			0x88,
			0x70

		};

		static BitmapCharRec ch186 = new BitmapCharRec(5, 6, -1, -5, 9, ch186data);
		//char: &Hb9 */

		static byte[] ch185data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0xc0,
			0x40

		};

		static BitmapCharRec ch185 = new BitmapCharRec(3, 6, -1, -4, 9, ch185data);
		//char: &Hb8 */

		static byte[] ch184data = {
			0x60,
			0x90,
			0x30

		};

		static BitmapCharRec ch184 = new BitmapCharRec(4, 3, -2, 3, 9, ch184data);
		//char: &Hb7 */

		static byte[] ch183data = {
			0xc0,
			0xc0

		};

		static BitmapCharRec ch183 = new BitmapCharRec(2, 2, -4, -4, 9, ch183data);
		//char: &Hb6 */

		static byte[] ch182data = {
			0xa,
			0xa,
			0xa,
			0xa,
			0xa,
			0x7a,
			0x8a,
			0x8a,
			0x8a,
			0x7e

		};

		static BitmapCharRec ch182 = new BitmapCharRec(7, 10, -1, 0, 9, ch182data);
		//char: &Hb5 */

		static byte[] ch181data = {
			0x80,
			0x80,
			0xba,
			0xc6,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82

		};

		static BitmapCharRec ch181 = new BitmapCharRec(7, 9, -1, 2, 9, ch181data);
		//char: &Hb4 */

		static byte[] ch180data = {
			0xc0,
			0x20

		};

		static BitmapCharRec ch180 = new BitmapCharRec(3, 2, -3, -9, 9, ch180data);
		//char: &Hb3 */

		static byte[] ch179data = {
			0x60,
			0x90,
			0x10,
			0x20,
			0x90,
			0x60

		};

		static BitmapCharRec ch179 = new BitmapCharRec(4, 6, -1, -4, 9, ch179data);
		//char: &Hb2 */

		static byte[] ch178data = {
			0xf0,
			0x80,
			0x60,
			0x10,
			0x90,
			0x60

		};

		static BitmapCharRec ch178 = new BitmapCharRec(4, 6, -1, -4, 9, ch178data);
		//char: &Hb1 */

		static byte[] ch177data = {
			0xfe,
			0x0,
			0x10,
			0x10,
			0x10,
			0xfe,
			0x10,
			0x10,
			0x10

		};

		static BitmapCharRec ch177 = new BitmapCharRec(7, 9, -1, -1, 9, ch177data);
		//char: &Hb0 */

		static byte[] ch176data = {
			0x60,
			0x90,
			0x90,
			0x60

		};

		static BitmapCharRec ch176 = new BitmapCharRec(4, 4, -3, -6, 9, ch176data);
		//char: &Haf */


		static byte[] ch175data = { 0xfc };

		static BitmapCharRec ch175 = new BitmapCharRec(6, 1, -1, -9, 9, ch175data);
		//char: &Hae */

		static byte[] ch174data = {
			0x3c,
			0x42,
			0xa5,
			0xa9,
			0xbd,
			0xa5,
			0xb9,
			0x42,
			0x3c

		};

		static BitmapCharRec ch174 = new BitmapCharRec(8, 9, 0, -1, 9, ch174data);
		//char: &Had */


		static byte[] ch173data = { 0xfc };

		static BitmapCharRec ch173 = new BitmapCharRec(6, 1, -1, -4, 9, ch173data);
		//char: &Hac */

		static byte[] ch172data = {
			0x4,
			0x4,
			0x4,
			0xfc

		};

		static BitmapCharRec ch172 = new BitmapCharRec(6, 4, -1, -2, 9, ch172data);
		//char: &Hab */

		static byte[] ch171data = {
			0x12,
			0x24,
			0x48,
			0x90,
			0x90,
			0x48,
			0x24,
			0x12

		};

		static BitmapCharRec ch171 = new BitmapCharRec(7, 8, -1, -1, 9, ch171data);
		//char: &Haa */

		static byte[] ch170data = {
			0xf8,
			0x0,
			0x78,
			0x90,
			0x70,
			0x90,
			0x60

		};

		static BitmapCharRec ch170 = new BitmapCharRec(5, 7, -3, -3, 9, ch170data);
		//char: &Ha9 */

		static byte[] ch169data = {
			0x3c,
			0x42,
			0x99,
			0xa5,
			0xa1,
			0xa5,
			0x99,
			0x42,
			0x3c

		};

		static BitmapCharRec ch169 = new BitmapCharRec(8, 9, 0, -1, 9, ch169data);
		//char: &Ha8 */

		static byte[] ch168data = {
			0xa0,
			0xa0

		};

		static BitmapCharRec ch168 = new BitmapCharRec(3, 2, -3, -9, 9, ch168data);
		//char: &Ha7 */

		static byte[] ch167data = {
			0x70,
			0x88,
			0x8,
			0x70,
			0x88,
			0x88,
			0x88,
			0x70,
			0x80,
			0x88,
			0x70

		};

		static BitmapCharRec ch167 = new BitmapCharRec(5, 11, -2, 1, 9, ch167data);
		//char: &Ha6 */

		static byte[] ch166data = {
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x0,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch166 = new BitmapCharRec(1, 11, -4, 1, 9, ch166data);
		//char: &Ha5 */

		static byte[] ch165data = {
			0x10,
			0x10,
			0x10,
			0x7c,
			0x10,
			0x7c,
			0x28,
			0x44,
			0x82,
			0x82

		};

		static BitmapCharRec ch165 = new BitmapCharRec(7, 10, -1, 0, 9, ch165data);
		//char: &Ha4 */

		static byte[] ch164data = {
			0x82,
			0x7c,
			0x44,
			0x44,
			0x7c,
			0x82

		};

		static BitmapCharRec ch164 = new BitmapCharRec(7, 6, -1, -3, 9, ch164data);
		//char: &Ha3 */

		static byte[] ch163data = {
			0x5c,
			0xa2,
			0x60,
			0x20,
			0x20,
			0xf8,
			0x20,
			0x20,
			0x22,
			0x1c

		};

		static BitmapCharRec ch163 = new BitmapCharRec(7, 10, -1, 0, 9, ch163data);
		//char: &Ha2 */

		static byte[] ch162data = {
			0x40,
			0x78,
			0xa4,
			0xa0,
			0x90,
			0x94,
			0x78,
			0x8

		};

		static BitmapCharRec ch162 = new BitmapCharRec(6, 8, -1, 0, 9, ch162data);
		//char: &Ha1 */

		static byte[] ch161data = {
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x0,
			0x0,
			0x80,
			0x80

		};

		static BitmapCharRec ch161 = new BitmapCharRec(1, 11, -4, 0, 9, ch161data);
		//char: &H7e '~' */

		static byte[] ch126data = {
			0x8c,
			0x92,
			0x62

		};

		static BitmapCharRec ch126 = new BitmapCharRec(7, 3, -1, -7, 9, ch126data);
		//char: &H7d '}' */

		static byte[] ch125data = {
			0xe0,
			0x10,
			0x10,
			0x10,
			0x20,
			0x18,
			0x18,
			0x20,
			0x10,
			0x10,
			0x10,
			0xe0

		};

		static BitmapCharRec ch125 = new BitmapCharRec(5, 12, -1, 1, 9, ch125data);
		//char: &H7c '|' */

		static byte[] ch124data = {
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch124 = new BitmapCharRec(1, 12, -4, 1, 9, ch124data);
		//char: &H7b '{' */

		static byte[] ch123data = {
			0x38,
			0x40,
			0x40,
			0x40,
			0x20,
			0xc0,
			0xc0,
			0x20,
			0x40,
			0x40,
			0x40,
			0x38

		};

		static BitmapCharRec ch123 = new BitmapCharRec(5, 12, -3, 1, 9, ch123data);
		//char: &H7a 'z' */

		static byte[] ch122data = {
			0xfe,
			0x40,
			0x20,
			0x10,
			0x8,
			0x4,
			0xfe

		};

		static BitmapCharRec ch122 = new BitmapCharRec(7, 7, -1, 0, 9, ch122data);
		//char: &H79 'y' */

		static byte[] ch121data = {
			0x78,
			0x84,
			0x4,
			0x74,
			0x8c,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84

		};

		static BitmapCharRec ch121 = new BitmapCharRec(6, 10, -1, 3, 9, ch121data);
		//char: &H78 'x' */

		static byte[] ch120data = {
			0x82,
			0x44,
			0x28,
			0x10,
			0x28,
			0x44,
			0x82

		};

		static BitmapCharRec ch120 = new BitmapCharRec(7, 7, -1, 0, 9, ch120data);
		//char: &H77 'w' */

		static byte[] ch119data = {
			0x44,
			0xaa,
			0x92,
			0x92,
			0x92,
			0x82,
			0x82

		};

		static BitmapCharRec ch119 = new BitmapCharRec(7, 7, -1, 0, 9, ch119data);
		//char: &H76 'v' */

		static byte[] ch118data = {
			0x10,
			0x28,
			0x28,
			0x44,
			0x44,
			0x82,
			0x82

		};

		static BitmapCharRec ch118 = new BitmapCharRec(7, 7, -1, 0, 9, ch118data);
		//char: &H75 'u' */

		static byte[] ch117data = {
			0x7a,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84,
			0x84

		};

		static BitmapCharRec ch117 = new BitmapCharRec(7, 7, -1, 0, 9, ch117data);
		//char: &H74 't' */

		static byte[] ch116data = {
			0x1c,
			0x22,
			0x20,
			0x20,
			0x20,
			0x20,
			0xfc,
			0x20,
			0x20

		};

		static BitmapCharRec ch116 = new BitmapCharRec(7, 9, -1, 0, 9, ch116data);
		//char: &H73 's' */

		static byte[] ch115data = {
			0x7c,
			0x82,
			0x2,
			0x7c,
			0x80,
			0x82,
			0x7c

		};

		static BitmapCharRec ch115 = new BitmapCharRec(7, 7, -1, 0, 9, ch115data);
		//char: &H72 'r' */

		static byte[] ch114data = {
			0x40,
			0x40,
			0x40,
			0x40,
			0x42,
			0x62,
			0x9c

		};

		static BitmapCharRec ch114 = new BitmapCharRec(7, 7, -1, 0, 9, ch114data);
		//char: &H71 'q' */

		static byte[] ch113data = {
			0x2,
			0x2,
			0x2,
			0x7a,
			0x86,
			0x82,
			0x82,
			0x82,
			0x86,
			0x7a

		};

		static BitmapCharRec ch113 = new BitmapCharRec(7, 10, -1, 3, 9, ch113data);
		//char: &H70 'p' */

		static byte[] ch112data = {
			0x80,
			0x80,
			0x80,
			0xbc,
			0xc2,
			0x82,
			0x82,
			0x82,
			0xc2,
			0xbc

		};

		static BitmapCharRec ch112 = new BitmapCharRec(7, 10, -1, 3, 9, ch112data);
		//char: &H6f 'o' */

		static byte[] ch111data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c

		};

		static BitmapCharRec ch111 = new BitmapCharRec(7, 7, -1, 0, 9, ch111data);
		//char: &H6e 'n' */

		static byte[] ch110data = {
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0xc2,
			0xbc

		};

		static BitmapCharRec ch110 = new BitmapCharRec(7, 7, -1, 0, 9, ch110data);
		//char: &H6d 'm' */

		static byte[] ch109data = {
			0x82,
			0x92,
			0x92,
			0x92,
			0x92,
			0x92,
			0xec

		};

		static BitmapCharRec ch109 = new BitmapCharRec(7, 7, -1, 0, 9, ch109data);
		//char: &H6c 'l' */

		static byte[] ch108data = {
			0xf8,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0xe0

		};

		static BitmapCharRec ch108 = new BitmapCharRec(5, 10, -2, 0, 9, ch108data);
		//char: &H6b 'k' */

		static byte[] ch107data = {
			0x82,
			0x8c,
			0xb0,
			0xc0,
			0xb0,
			0x8c,
			0x82,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch107 = new BitmapCharRec(7, 10, -1, 0, 9, ch107data);
		//char: &H6a 'j' */

		static byte[] ch106data = {
			0x78,
			0x84,
			0x84,
			0x84,
			0x4,
			0x4,
			0x4,
			0x4,
			0x4,
			0x1c,
			0x0,
			0x0,
			0xc

		};

		static BitmapCharRec ch106 = new BitmapCharRec(6, 13, -1, 3, 9, ch106data);
		//char: &H69 'i' */

		static byte[] ch105data = {
			0xf8,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0xe0,
			0x0,
			0x0,
			0x60

		};

		static BitmapCharRec ch105 = new BitmapCharRec(5, 10, -2, 0, 9, ch105data);
		//char: &H68 'h' */

		static byte[] ch104data = {
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0xc2,
			0xbc,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch104 = new BitmapCharRec(7, 10, -1, 0, 9, ch104data);
		//char: &H67 'g' */

		static byte[] ch103data = {
			0x7c,
			0x82,
			0x82,
			0x7c,
			0x80,
			0x78,
			0x84,
			0x84,
			0x84,
			0x7a

		};

		static BitmapCharRec ch103 = new BitmapCharRec(7, 10, -1, 3, 9, ch103data);
		//char: &H66 'f' */

		static byte[] ch102data = {
			0x20,
			0x20,
			0x20,
			0x20,
			0xf8,
			0x20,
			0x20,
			0x22,
			0x22,
			0x1c

		};

		static BitmapCharRec ch102 = new BitmapCharRec(7, 10, -1, 0, 9, ch102data);
		//char: &H65 'e' */

		static byte[] ch101data = {
			0x7c,
			0x80,
			0x80,
			0xfe,
			0x82,
			0x82,
			0x7c

		};

		static BitmapCharRec ch101 = new BitmapCharRec(7, 7, -1, 0, 9, ch101data);
		//char: &H64 'd' */

		static byte[] ch100data = {
			0x7a,
			0x86,
			0x82,
			0x82,
			0x82,
			0x86,
			0x7a,
			0x2,
			0x2,
			0x2

		};

		static BitmapCharRec ch100 = new BitmapCharRec(7, 10, -1, 0, 9, ch100data);
		//char: &H63 'c' */

		static byte[] ch99data = {
			0x7c,
			0x82,
			0x80,
			0x80,
			0x80,
			0x82,
			0x7c

		};

		static BitmapCharRec ch99 = new BitmapCharRec(7, 7, -1, 0, 9, ch99data);
		//char: &H62 'b' */

		static byte[] ch98data = {
			0xbc,
			0xc2,
			0x82,
			0x82,
			0x82,
			0xc2,
			0xbc,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch98 = new BitmapCharRec(7, 10, -1, 0, 9, ch98data);
		//char: &H61 'a' */

		static byte[] ch97data = {
			0x7a,
			0x86,
			0x82,
			0x7e,
			0x2,
			0x2,
			0x7c

		};

		static BitmapCharRec ch97 = new BitmapCharRec(7, 7, -1, 0, 9, ch97data);
		//char: &H60 '`' */

		static byte[] ch96data = {
			0x10,
			0x20,
			0x40,
			0xc0

		};

		static BitmapCharRec ch96 = new BitmapCharRec(4, 4, -3, -6, 9, ch96data);
		//char: &H5f '_' */


		static byte[] ch95data = { 0xff };

		static BitmapCharRec ch95 = new BitmapCharRec(8, 1, 0, 1, 9, ch95data);
		//char: &H5e '^' */

		static byte[] ch94data = {
			0x82,
			0x44,
			0x28,
			0x10

		};

		static BitmapCharRec ch94 = new BitmapCharRec(7, 4, -1, -6, 9, ch94data);
		//char: &H5d ']' */

		static byte[] ch93data = {
			0xf0,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0xf0

		};

		static BitmapCharRec ch93 = new BitmapCharRec(4, 12, -2, 1, 9, ch93data);
		//char: &H5c '\' */

		static byte[] ch92data = {
			0x2,
			0x4,
			0x4,
			0x8,
			0x10,
			0x10,
			0x20,
			0x40,
			0x40,
			0x80

		};

		static BitmapCharRec ch92 = new BitmapCharRec(7, 10, -1, 0, 9, ch92data);
		//char: &H5b '[' */

		static byte[] ch91data = {
			0xf0,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0xf0

		};

		static BitmapCharRec ch91 = new BitmapCharRec(4, 12, -3, 1, 9, ch91data);
		//char: &H5a 'Z' */

		static byte[] ch90data = {
			0xfe,
			0x80,
			0x80,
			0x40,
			0x20,
			0x10,
			0x8,
			0x4,
			0x2,
			0xfe

		};

		static BitmapCharRec ch90 = new BitmapCharRec(7, 10, -1, 0, 9, ch90data);
		//char: &H59 'Y' */

		static byte[] ch89data = {
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x28,
			0x44,
			0x82,
			0x82

		};

		static BitmapCharRec ch89 = new BitmapCharRec(7, 10, -1, 0, 9, ch89data);
		//char: &H58 'X' */

		static byte[] ch88data = {
			0x82,
			0x82,
			0x44,
			0x28,
			0x10,
			0x10,
			0x28,
			0x44,
			0x82,
			0x82

		};

		static BitmapCharRec ch88 = new BitmapCharRec(7, 10, -1, 0, 9, ch88data);
		//char: &H57 'W' */

		static byte[] ch87data = {
			0x44,
			0xaa,
			0x92,
			0x92,
			0x92,
			0x92,
			0x82,
			0x82,
			0x82,
			0x82

		};

		static BitmapCharRec ch87 = new BitmapCharRec(7, 10, -1, 0, 9, ch87data);
		//char: &H56 'V' */

		static byte[] ch86data = {
			0x10,
			0x28,
			0x28,
			0x28,
			0x44,
			0x44,
			0x44,
			0x82,
			0x82,
			0x82

		};

		static BitmapCharRec ch86 = new BitmapCharRec(7, 10, -1, 0, 9, ch86data);
		//char: &H55 'U' */

		static byte[] ch85data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82

		};

		static BitmapCharRec ch85 = new BitmapCharRec(7, 10, -1, 0, 9, ch85data);
		//char: &H54 'T' */

		static byte[] ch84data = {
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0xfe

		};

		static BitmapCharRec ch84 = new BitmapCharRec(7, 10, -1, 0, 9, ch84data);
		//char: &H53 'S' */

		static byte[] ch83data = {
			0x7c,
			0x82,
			0x82,
			0x2,
			0xc,
			0x70,
			0x80,
			0x82,
			0x82,
			0x7c

		};

		static BitmapCharRec ch83 = new BitmapCharRec(7, 10, -1, 0, 9, ch83data);
		//char: &H52 'R' */

		static byte[] ch82data = {
			0x82,
			0x82,
			0x84,
			0x88,
			0x90,
			0xfc,
			0x82,
			0x82,
			0x82,
			0xfc

		};

		static BitmapCharRec ch82 = new BitmapCharRec(7, 10, -1, 0, 9, ch82data);
		//char: &H51 'Q' */

		static byte[] ch81data = {
			0x6,
			0x8,
			0x7c,
			0x92,
			0xa2,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c

		};

		static BitmapCharRec ch81 = new BitmapCharRec(7, 12, -1, 2, 9, ch81data);
		//char: &H50 'P' */

		static byte[] ch80data = {
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0xfc,
			0x82,
			0x82,
			0x82,
			0xfc

		};

		static BitmapCharRec ch80 = new BitmapCharRec(7, 10, -1, 0, 9, ch80data);
		//char: &H4f 'O' */

		static byte[] ch79data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x7c

		};

		static BitmapCharRec ch79 = new BitmapCharRec(7, 10, -1, 0, 9, ch79data);
		//char: &H4e 'N' */

		static byte[] ch78data = {
			0x82,
			0x82,
			0x82,
			0x86,
			0x8a,
			0x92,
			0xa2,
			0xc2,
			0x82,
			0x82

		};

		static BitmapCharRec ch78 = new BitmapCharRec(7, 10, -1, 0, 9, ch78data);
		//char: &H4d 'M' */

		static byte[] ch77data = {
			0x82,
			0x82,
			0x82,
			0x92,
			0x92,
			0xaa,
			0xaa,
			0xc6,
			0x82,
			0x82

		};

		static BitmapCharRec ch77 = new BitmapCharRec(7, 10, -1, 0, 9, ch77data);
		//char: &H4c 'L' */

		static byte[] ch76data = {
			0xfe,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch76 = new BitmapCharRec(7, 10, -1, 0, 9, ch76data);
		//char: &H4b 'K' */

		static byte[] ch75data = {
			0x82,
			0x84,
			0x88,
			0x90,
			0xa0,
			0xe0,
			0x90,
			0x88,
			0x84,
			0x82

		};

		static BitmapCharRec ch75 = new BitmapCharRec(7, 10, -1, 0, 9, ch75data);
		//char: &H4a 'J' */

		static byte[] ch74data = {
			0x78,
			0x84,
			0x4,
			0x4,
			0x4,
			0x4,
			0x4,
			0x4,
			0x4,
			0x1e

		};

		static BitmapCharRec ch74 = new BitmapCharRec(7, 10, -1, 0, 9, ch74data);
		//char: &H49 'I' */

		static byte[] ch73data = {
			0xf8,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0xf8

		};

		static BitmapCharRec ch73 = new BitmapCharRec(5, 10, -2, 0, 9, ch73data);
		//char: &H48 '&H' */

		static byte[] ch72data = {
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0xfe,
			0x82,
			0x82,
			0x82,
			0x82

		};

		static BitmapCharRec ch72 = new BitmapCharRec(7, 10, -1, 0, 9, ch72data);
		//char: &H47 'G' */

		static byte[] ch71data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0x8e,
			0x80,
			0x80,
			0x80,
			0x82,
			0x7c

		};

		static BitmapCharRec ch71 = new BitmapCharRec(7, 10, -1, 0, 9, ch71data);
		//char: &H46 'F' */

		static byte[] ch70data = {
			0x40,
			0x40,
			0x40,
			0x40,
			0x40,
			0x78,
			0x40,
			0x40,
			0x40,
			0xfe

		};

		static BitmapCharRec ch70 = new BitmapCharRec(7, 10, -1, 0, 9, ch70data);
		//char: &H45 'E' */

		static byte[] ch69data = {
			0xfe,
			0x40,
			0x40,
			0x40,
			0x40,
			0x78,
			0x40,
			0x40,
			0x40,
			0xfe

		};

		static BitmapCharRec ch69 = new BitmapCharRec(7, 10, -1, 0, 9, ch69data);
		//char: &H44 'D' */

		static byte[] ch68data = {
			0xfc,
			0x42,
			0x42,
			0x42,
			0x42,
			0x42,
			0x42,
			0x42,
			0x42,
			0xfc

		};

		static BitmapCharRec ch68 = new BitmapCharRec(7, 10, -1, 0, 9, ch68data);
		//char: &H43 'C' */

		static byte[] ch67data = {
			0x7c,
			0x82,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x82,
			0x7c

		};

		static BitmapCharRec ch67 = new BitmapCharRec(7, 10, -1, 0, 9, ch67data);
		//char: &H42 'B' */

		static byte[] ch66data = {
			0xfc,
			0x42,
			0x42,
			0x42,
			0x42,
			0x7c,
			0x42,
			0x42,
			0x42,
			0xfc

		};

		static BitmapCharRec ch66 = new BitmapCharRec(7, 10, -1, 0, 9, ch66data);
		//char: &H41 'A' */

		static byte[] ch65data = {
			0x82,
			0x82,
			0x82,
			0xfe,
			0x82,
			0x82,
			0x82,
			0x44,
			0x28,
			0x10

		};

		static BitmapCharRec ch65 = new BitmapCharRec(7, 10, -1, 0, 9, ch65data);
		//char: &H40 '@' */

		static byte[] ch64data = {
			0x7c,
			0x80,
			0x80,
			0x9a,
			0xa6,
			0xa2,
			0x9e,
			0x82,
			0x82,
			0x7c

		};

		static BitmapCharRec ch64 = new BitmapCharRec(7, 10, -1, 0, 9, ch64data);
		//char: &H3f '?' */

		static byte[] ch63data = {
			0x10,
			0x0,
			0x10,
			0x10,
			0x8,
			0x4,
			0x2,
			0x82,
			0x82,
			0x7c

		};

		static BitmapCharRec ch63 = new BitmapCharRec(7, 10, -1, 0, 9, ch63data);
		//char: &H3e '>' */

		static byte[] ch62data = {
			0x80,
			0x40,
			0x20,
			0x10,
			0x8,
			0x8,
			0x10,
			0x20,
			0x40,
			0x80

		};

		static BitmapCharRec ch62 = new BitmapCharRec(5, 10, -2, 0, 9, ch62data);
		//char: &H3d '=' */

		static byte[] ch61data = {
			0xfe,
			0x0,
			0x0,
			0xfe

		};

		static BitmapCharRec ch61 = new BitmapCharRec(7, 4, -1, -2, 9, ch61data);
		//char: &H3c '<' */

		static byte[] ch60data = {
			0x8,
			0x10,
			0x20,
			0x40,
			0x80,
			0x80,
			0x40,
			0x20,
			0x10,
			0x8

		};

		static BitmapCharRec ch60 = new BitmapCharRec(5, 10, -2, 0, 9, ch60data);
		//char: &H3b ';' */

		static byte[] ch59data = {
			0x80,
			0x40,
			0x40,
			0xc0,
			0xc0,
			0x0,
			0x0,
			0x0,
			0xc0,
			0xc0

		};

		static BitmapCharRec ch59 = new BitmapCharRec(2, 10, -4, 3, 9, ch59data);
		//char: &H3a ':' */

		static byte[] ch58data = {
			0xc0,
			0xc0,
			0x0,
			0x0,
			0x0,
			0xc0,
			0xc0

		};

		static BitmapCharRec ch58 = new BitmapCharRec(2, 7, -4, 0, 9, ch58data);
		//char: &H39 '9' */

		static byte[] ch57data = {
			0x78,
			0x4,
			0x2,
			0x2,
			0x7a,
			0x86,
			0x82,
			0x82,
			0x82,
			0x7c

		};

		static BitmapCharRec ch57 = new BitmapCharRec(7, 10, -1, 0, 9, ch57data);
		//char: &H38 '8' */

		static byte[] ch56data = {
			0x38,
			0x44,
			0x82,
			0x82,
			0x44,
			0x38,
			0x44,
			0x82,
			0x44,
			0x38

		};

		static BitmapCharRec ch56 = new BitmapCharRec(7, 10, -1, 0, 9, ch56data);
		//char: &H37 '7' */

		static byte[] ch55data = {
			0x40,
			0x40,
			0x20,
			0x20,
			0x10,
			0x8,
			0x4,
			0x2,
			0x2,
			0xfe

		};

		static BitmapCharRec ch55 = new BitmapCharRec(7, 10, -1, 0, 9, ch55data);
		//char: &H36 '6' */

		static byte[] ch54data = {
			0x7c,
			0x82,
			0x82,
			0x82,
			0xc2,
			0xbc,
			0x80,
			0x80,
			0x40,
			0x3c

		};

		static BitmapCharRec ch54 = new BitmapCharRec(7, 10, -1, 0, 9, ch54data);
		//char: &H35 '5' */

		static byte[] ch53data = {
			0x7c,
			0x82,
			0x2,
			0x2,
			0x2,
			0xc2,
			0xbc,
			0x80,
			0x80,
			0xfe

		};

		static BitmapCharRec ch53 = new BitmapCharRec(7, 10, -1, 0, 9, ch53data);
		//char: &H34 '4' */

		static byte[] ch52data = {
			0x4,
			0x4,
			0x4,
			0xfe,
			0x84,
			0x44,
			0x24,
			0x14,
			0xc,
			0x4

		};

		static BitmapCharRec ch52 = new BitmapCharRec(7, 10, -1, 0, 9, ch52data);
		//char: &H33 '3' */

		static byte[] ch51data = {
			0x7c,
			0x82,
			0x2,
			0x2,
			0x2,
			0x1c,
			0x8,
			0x4,
			0x2,
			0xfe

		};

		static BitmapCharRec ch51 = new BitmapCharRec(7, 10, -1, 0, 9, ch51data);
		//char: &H32 '2' */

		static byte[] ch50data = {
			0xfe,
			0x80,
			0x40,
			0x30,
			0x8,
			0x4,
			0x2,
			0x82,
			0x82,
			0x7c

		};

		static BitmapCharRec ch50 = new BitmapCharRec(7, 10, -1, 0, 9, ch50data);
		//char: &H31 '1' */

		static byte[] ch49data = {
			0xfe,
			0x10,
			0x10,
			0x10,
			0x10,
			0x10,
			0x90,
			0x50,
			0x30,
			0x10

		};

		static BitmapCharRec ch49 = new BitmapCharRec(7, 10, -1, 0, 9, ch49data);
		//char: &H30 '0' */

		static byte[] ch48data = {
			0x38,
			0x44,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x82,
			0x44,
			0x38

		};

		static BitmapCharRec ch48 = new BitmapCharRec(7, 10, -1, 0, 9, ch48data);
		//char: &H2f '/' */

		static byte[] ch47data = {
			0x80,
			0x40,
			0x40,
			0x20,
			0x10,
			0x10,
			0x8,
			0x4,
			0x4,
			0x2

		};

		static BitmapCharRec ch47 = new BitmapCharRec(7, 10, -1, 0, 9, ch47data);
		//char: &H2e '.' */

		static byte[] ch46data = {
			0xc0,
			0xc0

		};

		static BitmapCharRec ch46 = new BitmapCharRec(2, 2, -4, 0, 9, ch46data);
		//char: &H2d '-' */


		static byte[] ch45data = { 0xfe };

		static BitmapCharRec ch45 = new BitmapCharRec(7, 1, -1, -4, 9, ch45data);
		//char: &H2c ',' */

		static byte[] ch44data = {
			0x80,
			0x40,
			0x40,
			0xc0,
			0xc0

		};

		static BitmapCharRec ch44 = new BitmapCharRec(2, 5, -4, 3, 9, ch44data);
		//char: &H2b '+' */

		static byte[] ch43data = {
			0x10,
			0x10,
			0x10,
			0xfe,
			0x10,
			0x10,
			0x10

		};

		static BitmapCharRec ch43 = new BitmapCharRec(7, 7, -1, -1, 9, ch43data);
		//char: &H2a '*' */

		static byte[] ch42data = {
			0x10,
			0x92,
			0x54,
			0x38,
			0x54,
			0x92,
			0x10

		};

		static BitmapCharRec ch42 = new BitmapCharRec(7, 7, -1, -1, 9, ch42data);
		//char: &H29 ')' */

		static byte[] ch41data = {
			0x80,
			0x40,
			0x40,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x20,
			0x40,
			0x40,
			0x80

		};

		static BitmapCharRec ch41 = new BitmapCharRec(3, 12, -3, 1, 9, ch41data);
		//char: &H28 '(' */

		static byte[] ch40data = {
			0x20,
			0x40,
			0x40,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x40,
			0x40,
			0x20

		};

		static BitmapCharRec ch40 = new BitmapCharRec(3, 12, -3, 1, 9, ch40data);
		//char: &H27 ''' */

		static byte[] ch39data = {
			0x80,
			0x40,
			0x20,
			0x30

		};

		static BitmapCharRec ch39 = new BitmapCharRec(4, 4, -3, -6, 9, ch39data);
		//char: &H26 '&' */

		static byte[] ch38data = {
			0x62,
			0x94,
			0x88,
			0x94,
			0x62,
			0x60,
			0x90,
			0x90,
			0x90,
			0x60

		};

		static BitmapCharRec ch38 = new BitmapCharRec(7, 10, -1, 0, 9, ch38data);
		//char: &H25 '%' */

		static byte[] ch37data = {
			0x84,
			0x4a,
			0x4a,
			0x24,
			0x10,
			0x10,
			0x48,
			0xa4,
			0xa4,
			0x42

		};

		static BitmapCharRec ch37 = new BitmapCharRec(7, 10, -1, 0, 9, ch37data);
		//char: &H24 '$' */

		static byte[] ch36data = {
			0x10,
			0x7c,
			0x92,
			0x12,
			0x12,
			0x14,
			0x38,
			0x50,
			0x90,
			0x92,
			0x7c,
			0x10

		};

		static BitmapCharRec ch36 = new BitmapCharRec(7, 12, -1, 1, 9, ch36data);
		//char: &H23 '#' */

		static byte[] ch35data = {
			0x48,
			0x48,
			0xfc,
			0x48,
			0x48,
			0xfc,
			0x48,
			0x48

		};

		static BitmapCharRec ch35 = new BitmapCharRec(6, 8, -1, -1, 9, ch35data);
		//char: &H22 '"' */

		static byte[] ch34data = {
			0x90,
			0x90,
			0x90

		};

		static BitmapCharRec ch34 = new BitmapCharRec(4, 3, -3, -7, 9, ch34data);
		//char: &H21 '!' */

		static byte[] ch33data = {
			0x80,
			0x80,
			0x0,
			0x0,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch33 = new BitmapCharRec(1, 11, -4, 0, 9, ch33data);
		//char: &H1f */

		static byte[] ch31data = {
			0xc0,
			0xc0

		};

		static BitmapCharRec ch31 = new BitmapCharRec(2, 2, -4, -2, 9, ch31data);
		//char: &H1e */

		static byte[] ch30data = {
			0x5c,
			0xa2,
			0x60,
			0x20,
			0x20,
			0xf8,
			0x20,
			0x20,
			0x22,
			0x1c

		};

		static BitmapCharRec ch30 = new BitmapCharRec(7, 10, -1, 0, 9, ch30data);
		//char: &H1d */

		static byte[] ch29data = {
			0x80,
			0x40,
			0xfe,
			0x10,
			0xfe,
			0x4,
			0x2

		};

		static BitmapCharRec ch29 = new BitmapCharRec(7, 7, -1, 0, 9, ch29data);
		//char: &H1c */

		static byte[] ch28data = {
			0x44,
			0x24,
			0x24,
			0x24,
			0x24,
			0x24,
			0xfe

		};

		static BitmapCharRec ch28 = new BitmapCharRec(7, 7, -1, 0, 9, ch28data);
		//char: &H1b */

		static byte[] ch27data = {
			0xfe,
			0x0,
			0x80,
			0x40,
			0x20,
			0x10,
			0x8,
			0x8,
			0x10,
			0x20,
			0x40,
			0x80

		};

		static BitmapCharRec ch27 = new BitmapCharRec(7, 12, -1, 2, 9, ch27data);
		//char: &H1a */

		static byte[] ch26data = {
			0xfc,
			0x0,
			0x4,
			0x8,
			0x10,
			0x20,
			0x40,
			0x40,
			0x20,
			0x10,
			0x8,
			0x4

		};

		static BitmapCharRec ch26 = new BitmapCharRec(6, 12, -2, 2, 9, ch26data);
		//char: &H19 */

		static byte[] ch25data = {
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch25 = new BitmapCharRec(1, 15, -4, 3, 9, ch25data);
		//char: &H18 */

		static byte[] ch24data = {
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0xff,
			0x80

		};

		static BitmapCharRec ch24 = new BitmapCharRec(9, 7, 0, 3, 9, ch24data);
		//char: &H17 */

		static byte[] ch23data = {
			0xff,
			0x80,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0

		};

		static BitmapCharRec ch23 = new BitmapCharRec(9, 9, 0, -3, 9, ch23data);
		//char: &H16 */

		static byte[] ch22data = {
			0x8,
			0x8,
			0x8,
			0x8,
			0x8,
			0x8,
			0xf8,
			0x8,
			0x8,
			0x8,
			0x8,
			0x8,
			0x8,
			0x8,
			0x8

		};

		static BitmapCharRec ch22 = new BitmapCharRec(5, 15, 0, 3, 9, ch22data);
		//char: &H15 */

		static byte[] ch21data = {
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0xf8,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch21 = new BitmapCharRec(5, 15, -4, 3, 9, ch21data);
		//char: &H14 */

		static byte[] ch20data = {
			0xff,
			0x80

		};

		static BitmapCharRec ch20 = new BitmapCharRec(9, 1, 0, 1, 9, ch20data);
		//char: &H13 */

		static byte[] ch19data = {
			0xff,
			0x80

		};

		static BitmapCharRec ch19 = new BitmapCharRec(9, 1, 0, -1, 9, ch19data);
		//char: &H12 */

		static byte[] ch18data = {
			0xff,
			0x80

		};

		static BitmapCharRec ch18 = new BitmapCharRec(9, 1, 0, -3, 9, ch18data);
		//char: &H11 */

		static byte[] ch17data = {
			0xff,
			0x80

		};

		static BitmapCharRec ch17 = new BitmapCharRec(9, 1, 0, -5, 9, ch17data);
		//char: &H10 */

		static byte[] ch16data = {
			0xff,
			0x80

		};

		static BitmapCharRec ch16 = new BitmapCharRec(9, 1, 0, -7, 9, ch16data);
		//char: &Hf */

		static byte[] ch15data = {
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0xff,
			0x80,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0,
			0x8,
			0x0

		};

		static BitmapCharRec ch15 = new BitmapCharRec(9, 15, 0, 3, 9, ch15data);
		//char: &He */

		static byte[] ch14data = {
			0xf8,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch14 = new BitmapCharRec(5, 9, -4, -3, 9, ch14data);
		//char: &Hd */

		static byte[] ch13data = {
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0xf8

		};

		static BitmapCharRec ch13 = new BitmapCharRec(5, 7, -4, 3, 9, ch13data);
		//char: &Hc */

		static byte[] ch12data = {
			0x8,
			0x8,
			0x8,
			0x8,
			0x8,
			0x8,
			0xf8

		};

		static BitmapCharRec ch12 = new BitmapCharRec(5, 7, 0, 3, 9, ch12data);
		//char: &Hb */

		static byte[] ch11data = {
			0xf8,
			0x8,
			0x8,
			0x8,
			0x8,
			0x8,
			0x8,
			0x8,
			0x8

		};

		static BitmapCharRec ch11 = new BitmapCharRec(5, 9, 0, -3, 9, ch11data);
		//char: &Ha */

		static byte[] ch10data = {
			0x8,
			0x8,
			0x8,
			0x8,
			0x3e,
			0x0,
			0x20,
			0x50,
			0x88,
			0x88

		};

		static BitmapCharRec ch10 = new BitmapCharRec(7, 10, -1, 2, 9, ch10data);
		//char: &H9 */

		static byte[] ch9data = {
			0x3e,
			0x20,
			0x20,
			0x20,
			0x20,
			0x88,
			0x98,
			0xa8,
			0xc8,
			0x88

		};

		static BitmapCharRec ch9 = new BitmapCharRec(7, 10, -1, 2, 9, ch9data);
		//char: &H8 */

		static byte[] ch8data = {
			0xfe,
			0x10,
			0x10,
			0xfe,
			0x10,
			0x10

		};

		static BitmapCharRec ch8 = new BitmapCharRec(7, 6, -1, 0, 9, ch8data);
		//char: &H7 */

		static byte[] ch7data = {
			0x70,
			0x88,
			0x88,
			0x70

		};

		static BitmapCharRec ch7 = new BitmapCharRec(5, 4, -2, -6, 9, ch7data);
		//char: &H6 */

		static byte[] ch6data = {
			0x20,
			0x20,
			0x3c,
			0x20,
			0x3e,
			0x0,
			0xf8,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch6 = new BitmapCharRec(7, 10, -1, 2, 9, ch6data);
		//char: &H5 */

		static byte[] ch5data = {
			0x22,
			0x22,
			0x3c,
			0x22,
			0x3c,
			0x0,
			0x78,
			0x80,
			0x80,
			0x78

		};

		static BitmapCharRec ch5 = new BitmapCharRec(7, 10, -1, 2, 9, ch5data);
		//char: &H4 */

		static byte[] ch4data = {
			0x10,
			0x10,
			0x1c,
			0x10,
			0x1e,
			0x80,
			0x80,
			0xe0,
			0x80,
			0xf0

		};

		static BitmapCharRec ch4 = new BitmapCharRec(7, 10, -1, 2, 9, ch4data);
		//char: &H3 */

		static byte[] ch3data = {
			0x8,
			0x8,
			0x8,
			0x3e,
			0x0,
			0x88,
			0x88,
			0xf8,
			0x88,
			0x88

		};

		static BitmapCharRec ch3 = new BitmapCharRec(7, 10, -1, 2, 9, ch3data);
		//char: &H2 */

		static byte[] ch2data = {
			0x55,
			0xaa,
			0x55,
			0xaa,
			0x55,
			0xaa,
			0x55,
			0xaa,
			0x55,
			0xaa,
			0x55,
			0xaa,
			0x55,
			0xaa

		};

		static BitmapCharRec ch2 = new BitmapCharRec(8, 14, 0, 3, 9, ch2data);
		//char: &H1 */

		static byte[] ch1data = {
			0x10,
			0x38,
			0x7c,
			0xfe,
			0x7c,
			0x38,
			0x10

		};

		static BitmapCharRec ch1 = new BitmapCharRec(7, 7, -1, 0, 9, ch1data);
		static BitmapCharRec[] chars = {
			ch0,
			ch1,
			ch2,
			ch3,
			ch4,
			ch5,
			ch6,
			ch7,
			ch8,
			ch9,
			ch10,
			ch11,
			ch12,
			ch13,
			ch14,
			ch15,
			ch16,
			ch17,
			ch18,
			ch19,
			ch20,
			ch21,
			ch22,
			ch23,
			ch24,
			ch25,
			ch26,
			ch27,
			ch28,
			ch29,
			ch30,
			ch31,
			ch32,
			ch33,
			ch34,
			ch35,
			ch36,
			ch37,
			ch38,
			ch39,
			ch40,
			ch41,
			ch42,
			ch43,
			ch44,
			ch45,
			ch46,
			ch47,
			ch48,
			ch49,
			ch50,
			ch51,
			ch52,
			ch53,
			ch54,
			ch55,
			ch56,
			ch57,
			ch58,
			ch59,
			ch60,
			ch61,
			ch62,
			ch63,
			ch64,
			ch65,
			ch66,
			ch67,
			ch68,
			ch69,
			ch70,
			ch71,
			ch72,
			ch73,
			ch74,
			ch75,
			ch76,
			ch77,
			ch78,
			ch79,
			ch80,
			ch81,
			ch82,
			ch83,
			ch84,
			ch85,
			ch86,
			ch87,
			ch88,
			ch89,
			ch90,
			ch91,
			ch92,
			ch93,
			ch94,
			ch95,
			ch96,
			ch97,
			ch98,
			ch99,
			ch100,
			ch101,
			ch102,
			ch103,
			ch104,
			ch105,
			ch106,
			ch107,
			ch108,
			ch109,
			ch110,
			ch111,
			ch112,
			ch113,
			ch114,
			ch115,
			ch116,
			ch117,
			ch118,
			ch119,
			ch120,
			ch121,
			ch122,
			ch123,
			ch124,
			ch125,
			ch126,
			ch127,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			ch160,
			ch161,
			ch162,
			ch163,
			ch164,
			ch165,
			ch166,
			ch167,
			ch168,
			ch169,
			ch170,
			ch171,
			ch172,
			ch173,
			ch174,
			ch175,
			ch176,
			ch177,
			ch178,
			ch179,
			ch180,
			ch181,
			ch182,
			ch183,
			ch184,
			ch185,
			ch186,
			ch187,
			ch188,
			ch189,
			ch190,
			ch191,
			ch192,
			ch193,
			ch194,
			ch195,
			ch196,
			ch197,
			ch198,
			ch199,
			ch200,
			ch201,
			ch202,
			ch203,
			ch204,
			ch205,
			ch206,
			ch207,
			ch208,
			ch209,
			ch210,
			ch211,
			ch212,
			ch213,
			ch214,
			ch215,
			ch216,
			ch217,
			ch218,
			ch219,
			ch220,
			ch221,
			ch222,
			ch223,
			ch224,
			ch225,
			ch226,
			ch227,
			ch228,
			ch229,
			ch230,
			ch231,
			ch232,
			ch233,
			ch234,
			ch235,
			ch236,
			ch237,
			ch238,
			ch239,
			ch240,
			ch241,
			ch242,
			ch243,
			ch244,
			ch245,
			ch246,
			ch247,
			ch248,
			ch249,
			ch250,
			ch251,
			ch252,
			ch253,
			ch254,
			ch255

		};
		public static BitmapFontRec glutBitmap9By15 = new BitmapFontRec("-misc-fixed-medium-r-normal--15-140-75-75-C-90-iso8859-1", 256, 0, chars);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
