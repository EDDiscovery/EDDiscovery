
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace nzy3D.Glut
{

	public class GLUTBitmapTimesRoman10
	{

		// char: 0xff */

		static byte[] ch255data = {
			0x80,
			0xc0,
			0x40,
			0x60,
			0xa0,
			0x90,
			0xb8,
			0x0,
			0xa0

		};

		static BitmapCharRec ch255 = new BitmapCharRec(5, 9, 0, 2, 5, ch255data);
		// char: 0xfe */

		static byte[] ch254data = {
			0xc0,
			0x80,
			0xe0,
			0x90,
			0x90,
			0x90,
			0xe0,
			0x80,
			0x80

		};

		static BitmapCharRec ch254 = new BitmapCharRec(4, 9, 0, 2, 5, ch254data);
		// char: 0xfd */

		static byte[] ch253data = {
			0x80,
			0xc0,
			0x40,
			0x60,
			0xa0,
			0x90,
			0xb8,
			0x0,
			0x20,
			0x10

		};

		static BitmapCharRec ch253 = new BitmapCharRec(5, 10, 0, 2, 5, ch253data);
		// char: 0xfc */

		static byte[] ch252data = {
			0x68,
			0x90,
			0x90,
			0x90,
			0x90,
			0x0,
			0x50

		};

		static BitmapCharRec ch252 = new BitmapCharRec(5, 7, 0, 0, 5, ch252data);
		// char: 0xfb */

		static byte[] ch251data = {
			0x68,
			0x90,
			0x90,
			0x90,
			0x90,
			0x0,
			0x50,
			0x20

		};

		static BitmapCharRec ch251 = new BitmapCharRec(5, 8, 0, 0, 5, ch251data);
		// char: 0xfa */

		static byte[] ch250data = {
			0x68,
			0x90,
			0x90,
			0x90,
			0x90,
			0x0,
			0x40,
			0x20

		};

		static BitmapCharRec ch250 = new BitmapCharRec(5, 8, 0, 0, 5, ch250data);
		// char: 0xf9 */

		static byte[] ch249data = {
			0x68,
			0x90,
			0x90,
			0x90,
			0x90,
			0x0,
			0x20,
			0x40

		};

		static BitmapCharRec ch249 = new BitmapCharRec(5, 8, 0, 0, 5, ch249data);
		// char: 0xf8 */

		static byte[] ch248data = {
			0x80,
			0x70,
			0x48,
			0x48,
			0x48,
			0x38,
			0x4

		};

		static BitmapCharRec ch248 = new BitmapCharRec(6, 7, 1, 1, 5, ch248data);
		// char: 0xf7 */

		static byte[] ch247data = {
			0x20,
			0x0,
			0xf8,
			0x0,
			0x20

		};

		static BitmapCharRec ch247 = new BitmapCharRec(5, 5, 0, 0, 6, ch247data);
		// char: 0xf6 */

		static byte[] ch246data = {
			0x60,
			0x90,
			0x90,
			0x90,
			0x60,
			0x0,
			0xa0

		};

		static BitmapCharRec ch246 = new BitmapCharRec(4, 7, 0, 0, 5, ch246data);
		// char: 0xf5 */

		static byte[] ch245data = {
			0x60,
			0x90,
			0x90,
			0x90,
			0x60,
			0x0,
			0xa0,
			0x50

		};

		static BitmapCharRec ch245 = new BitmapCharRec(4, 8, 0, 0, 5, ch245data);
		// char: 0xf4 */

		static byte[] ch244data = {
			0x60,
			0x90,
			0x90,
			0x90,
			0x60,
			0x0,
			0xa0,
			0x40

		};

		static BitmapCharRec ch244 = new BitmapCharRec(4, 8, 0, 0, 5, ch244data);
		// char: 0xf3 */

		static byte[] ch243data = {
			0x60,
			0x90,
			0x90,
			0x90,
			0x60,
			0x0,
			0x40,
			0x20

		};

		static BitmapCharRec ch243 = new BitmapCharRec(4, 8, 0, 0, 5, ch243data);
		// char: 0xf2 */

		static byte[] ch242data = {
			0x60,
			0x90,
			0x90,
			0x90,
			0x60,
			0x0,
			0x20,
			0x40

		};

		static BitmapCharRec ch242 = new BitmapCharRec(4, 8, 0, 0, 5, ch242data);
		// char: 0xf1 */

		static byte[] ch241data = {
			0xd8,
			0x90,
			0x90,
			0x90,
			0xe0,
			0x0,
			0xa0,
			0x50

		};

		static BitmapCharRec ch241 = new BitmapCharRec(5, 8, 0, 0, 5, ch241data);
		// char: 0xf0 */

		static byte[] ch240data = {
			0x60,
			0x90,
			0x90,
			0x90,
			0x70,
			0xa0,
			0x70,
			0x40

		};

		static BitmapCharRec ch240 = new BitmapCharRec(4, 8, 0, 0, 5, ch240data);
		// char: 0xef */

		static byte[] ch239data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0xc0,
			0x0,
			0xa0

		};

		static BitmapCharRec ch239 = new BitmapCharRec(3, 7, 0, 0, 4, ch239data);
		// char: 0xee */

		static byte[] ch238data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0xc0,
			0x0,
			0xa0,
			0x40

		};

		static BitmapCharRec ch238 = new BitmapCharRec(3, 8, 0, 0, 4, ch238data);
		// char: 0xed */

		static byte[] ch237data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0xc0,
			0x0,
			0x40,
			0x20

		};

		static BitmapCharRec ch237 = new BitmapCharRec(3, 8, 0, 0, 4, ch237data);
		// char: 0xec */

		static byte[] ch236data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0xc0,
			0x0,
			0x40,
			0x80

		};

		static BitmapCharRec ch236 = new BitmapCharRec(3, 8, 0, 0, 4, ch236data);
		// char: 0xeb */

		static byte[] ch235data = {
			0x60,
			0x80,
			0xc0,
			0xa0,
			0x60,
			0x0,
			0xa0

		};

		static BitmapCharRec ch235 = new BitmapCharRec(3, 7, 0, 0, 4, ch235data);
		// char: 0xea */

		static byte[] ch234data = {
			0x60,
			0x80,
			0xc0,
			0xa0,
			0x60,
			0x0,
			0xa0,
			0x40

		};

		static BitmapCharRec ch234 = new BitmapCharRec(3, 8, 0, 0, 4, ch234data);
		// char: 0xe9 */

		static byte[] ch233data = {
			0x60,
			0x80,
			0xc0,
			0xa0,
			0x60,
			0x0,
			0x40,
			0x20

		};

		static BitmapCharRec ch233 = new BitmapCharRec(3, 8, 0, 0, 4, ch233data);
		// char: 0xe8 */

		static byte[] ch232data = {
			0x60,
			0x80,
			0xc0,
			0xa0,
			0x60,
			0x0,
			0x40,
			0x80

		};

		static BitmapCharRec ch232 = new BitmapCharRec(3, 8, 0, 0, 4, ch232data);
		// char: 0xe7 */

		static byte[] ch231data = {
			0xc0,
			0x20,
			0x40,
			0x60,
			0x80,
			0x80,
			0x80,
			0x60

		};

		static BitmapCharRec ch231 = new BitmapCharRec(3, 8, 0, 3, 4, ch231data);
		// char: 0xe6 */

		static byte[] ch230data = {
			0xd8,
			0xa0,
			0x70,
			0x28,
			0xd8

		};

		static BitmapCharRec ch230 = new BitmapCharRec(5, 5, 0, 0, 6, ch230data);
		// char: 0xe5 */

		static byte[] ch229data = {
			0xe0,
			0xa0,
			0x60,
			0x20,
			0xc0,
			0x40,
			0xa0,
			0x40

		};

		static BitmapCharRec ch229 = new BitmapCharRec(3, 8, 0, 0, 4, ch229data);
		// char: 0xe4 */

		static byte[] ch228data = {
			0xe0,
			0xa0,
			0x60,
			0x20,
			0xc0,
			0x0,
			0xa0

		};

		static BitmapCharRec ch228 = new BitmapCharRec(3, 7, 0, 0, 4, ch228data);
		// char: 0xe3 */

		static byte[] ch227data = {
			0xe0,
			0xa0,
			0x60,
			0x20,
			0xc0,
			0x0,
			0xa0,
			0x50

		};

		static BitmapCharRec ch227 = new BitmapCharRec(4, 8, 0, 0, 4, ch227data);
		// char: 0xe2 */

		static byte[] ch226data = {
			0xe0,
			0xa0,
			0x60,
			0x20,
			0xc0,
			0x0,
			0xa0,
			0x40

		};

		static BitmapCharRec ch226 = new BitmapCharRec(3, 8, 0, 0, 4, ch226data);
		// char: 0xe1 */

		static byte[] ch225data = {
			0xe0,
			0xa0,
			0x60,
			0x20,
			0xc0,
			0x0,
			0x40,
			0x20

		};

		static BitmapCharRec ch225 = new BitmapCharRec(3, 8, 0, 0, 4, ch225data);
		// char: 0xe0 */

		static byte[] ch224data = {
			0xe0,
			0xa0,
			0x60,
			0x20,
			0xc0,
			0x0,
			0x40,
			0x80

		};

		static BitmapCharRec ch224 = new BitmapCharRec(3, 8, 0, 0, 4, ch224data);
		// char: 0xdf */

		static byte[] ch223data = {
			0xe0,
			0x50,
			0x50,
			0x60,
			0x50,
			0x50,
			0x20

		};

		static BitmapCharRec ch223 = new BitmapCharRec(4, 7, 0, 0, 5, ch223data);
		// char: 0xde */

		static byte[] ch222data = {
			0xe0,
			0x40,
			0x70,
			0x48,
			0x70,
			0x40,
			0xe0

		};

		static BitmapCharRec ch222 = new BitmapCharRec(5, 7, 0, 0, 6, ch222data);
		// char: 0xdd */

		static byte[] ch221data = {
			0x38,
			0x10,
			0x10,
			0x28,
			0x28,
			0x44,
			0xee,
			0x0,
			0x10,
			0x8

		};

		static BitmapCharRec ch221 = new BitmapCharRec(7, 10, 0, 0, 8, ch221data);
		// char: 0xdc */

		static byte[] ch220data = {
			0x38,
			0x6c,
			0x44,
			0x44,
			0x44,
			0x44,
			0xee,
			0x0,
			0x28

		};

		static BitmapCharRec ch220 = new BitmapCharRec(7, 9, 0, 0, 8, ch220data);
		// char: 0xdb */

		static byte[] ch219data = {
			0x38,
			0x6c,
			0x44,
			0x44,
			0x44,
			0x44,
			0xee,
			0x0,
			0x28,
			0x10

		};

		static BitmapCharRec ch219 = new BitmapCharRec(7, 10, 0, 0, 8, ch219data);
		// char: 0xda */

		static byte[] ch218data = {
			0x38,
			0x6c,
			0x44,
			0x44,
			0x44,
			0x44,
			0xee,
			0x0,
			0x10,
			0x8

		};

		static BitmapCharRec ch218 = new BitmapCharRec(7, 10, 0, 0, 8, ch218data);
		// char: 0xd9 */

		static byte[] ch217data = {
			0x38,
			0x6c,
			0x44,
			0x44,
			0x44,
			0x44,
			0xee,
			0x0,
			0x10,
			0x20

		};

		static BitmapCharRec ch217 = new BitmapCharRec(7, 10, 0, 0, 8, ch217data);
		// char: 0xd8 */

		static byte[] ch216data = {
			0x80,
			0x7c,
			0x66,
			0x52,
			0x52,
			0x4a,
			0x66,
			0x3e,
			0x1

		};

		static BitmapCharRec ch216 = new BitmapCharRec(8, 9, 0, 1, 8, ch216data);
		// char: 0xd7 */

		static byte[] ch215data = {
			0x88,
			0x50,
			0x20,
			0x50,
			0x88

		};

		static BitmapCharRec ch215 = new BitmapCharRec(5, 5, 0, 0, 6, ch215data);
		// char: 0xd6 */

		static byte[] ch214data = {
			0x78,
			0xcc,
			0x84,
			0x84,
			0x84,
			0xcc,
			0x78,
			0x0,
			0x50

		};

		static BitmapCharRec ch214 = new BitmapCharRec(6, 9, 0, 0, 7, ch214data);
		// char: 0xd5 */

		static byte[] ch213data = {
			0x78,
			0xcc,
			0x84,
			0x84,
			0x84,
			0xcc,
			0x78,
			0x0,
			0x50,
			0x28

		};

		static BitmapCharRec ch213 = new BitmapCharRec(6, 10, 0, 0, 7, ch213data);
		// char: 0xd4 */

		static byte[] ch212data = {
			0x78,
			0xcc,
			0x84,
			0x84,
			0x84,
			0xcc,
			0x78,
			0x0,
			0x50,
			0x20

		};

		static BitmapCharRec ch212 = new BitmapCharRec(6, 10, 0, 0, 7, ch212data);
		// char: 0xd3 */

		static byte[] ch211data = {
			0x78,
			0xcc,
			0x84,
			0x84,
			0x84,
			0xcc,
			0x78,
			0x0,
			0x10,
			0x8

		};

		static BitmapCharRec ch211 = new BitmapCharRec(6, 10, 0, 0, 7, ch211data);
		// char: 0xd2 */

		static byte[] ch210data = {
			0x78,
			0xcc,
			0x84,
			0x84,
			0x84,
			0xcc,
			0x78,
			0x0,
			0x20,
			0x40

		};

		static BitmapCharRec ch210 = new BitmapCharRec(6, 10, 0, 0, 7, ch210data);
		// char: 0xd1 */

		static byte[] ch209data = {
			0xe4,
			0x4c,
			0x4c,
			0x54,
			0x54,
			0x64,
			0xee,
			0x0,
			0x50,
			0x28

		};

		static BitmapCharRec ch209 = new BitmapCharRec(7, 10, 0, 0, 8, ch209data);
		// char: 0xd0 */

		static byte[] ch208data = {
			0xf8,
			0x4c,
			0x44,
			0xe4,
			0x44,
			0x4c,
			0xf8

		};

		static BitmapCharRec ch208 = new BitmapCharRec(6, 7, 0, 0, 7, ch208data);
		// char: 0xcf */

		static byte[] ch207data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0x40,
			0x40,
			0xe0,
			0x0,
			0xa0

		};

		static BitmapCharRec ch207 = new BitmapCharRec(3, 9, 0, 0, 4, ch207data);
		// char: 0xce */

		static byte[] ch206data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0x40,
			0x40,
			0xe0,
			0x0,
			0xa0,
			0x40

		};

		static BitmapCharRec ch206 = new BitmapCharRec(3, 10, 0, 0, 4, ch206data);
		// char: 0xcd */

		static byte[] ch205data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0x40,
			0x40,
			0xe0,
			0x0,
			0x40,
			0x20

		};

		static BitmapCharRec ch205 = new BitmapCharRec(3, 10, 0, 0, 4, ch205data);
		// char: 0xcc */

		static byte[] ch204data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0x40,
			0x40,
			0xe0,
			0x0,
			0x40,
			0x80

		};

		static BitmapCharRec ch204 = new BitmapCharRec(3, 10, 0, 0, 4, ch204data);
		// char: 0xcb */

		static byte[] ch203data = {
			0xf8,
			0x48,
			0x40,
			0x70,
			0x40,
			0x48,
			0xf8,
			0x0,
			0x50

		};

		static BitmapCharRec ch203 = new BitmapCharRec(5, 9, 0, 0, 6, ch203data);
		// char: 0xca */

		static byte[] ch202data = {
			0xf8,
			0x48,
			0x40,
			0x70,
			0x40,
			0x48,
			0xf8,
			0x0,
			0x50,
			0x20

		};

		static BitmapCharRec ch202 = new BitmapCharRec(5, 10, 0, 0, 6, ch202data);
		// char: 0xc9 */

		static byte[] ch201data = {
			0xf8,
			0x48,
			0x40,
			0x70,
			0x40,
			0x48,
			0xf8,
			0x0,
			0x20,
			0x10

		};

		static BitmapCharRec ch201 = new BitmapCharRec(5, 10, 0, 0, 6, ch201data);
		// char: 0xc8 */

		static byte[] ch200data = {
			0xf8,
			0x48,
			0x40,
			0x70,
			0x40,
			0x48,
			0xf8,
			0x0,
			0x20,
			0x40

		};

		static BitmapCharRec ch200 = new BitmapCharRec(5, 10, 0, 0, 6, ch200data);
		// char: 0xc7 */

		static byte[] ch199data = {
			0x60,
			0x10,
			0x20,
			0x78,
			0xc4,
			0x80,
			0x80,
			0x80,
			0xc4,
			0x7c

		};

		static BitmapCharRec ch199 = new BitmapCharRec(6, 10, 0, 3, 7, ch199data);
		// char: 0xc6 */

		static byte[] ch198data = {
			0xef,
			0x49,
			0x78,
			0x2e,
			0x28,
			0x39,
			0x1f

		};

		static BitmapCharRec ch198 = new BitmapCharRec(8, 7, 0, 0, 9, ch198data);
		// char: 0xc5 */

		static byte[] ch197data = {
			0xee,
			0x44,
			0x7c,
			0x28,
			0x28,
			0x38,
			0x10,
			0x10,
			0x28,
			0x10

		};

		static BitmapCharRec ch197 = new BitmapCharRec(7, 10, 0, 0, 8, ch197data);
		// char: 0xc4 */

		static byte[] ch196data = {
			0xee,
			0x44,
			0x7c,
			0x28,
			0x28,
			0x38,
			0x10,
			0x0,
			0x28

		};

		static BitmapCharRec ch196 = new BitmapCharRec(7, 9, 0, 0, 8, ch196data);
		// char: 0xc3 */

		static byte[] ch195data = {
			0xee,
			0x44,
			0x7c,
			0x28,
			0x28,
			0x38,
			0x10,
			0x0,
			0x28,
			0x14

		};

		static BitmapCharRec ch195 = new BitmapCharRec(7, 10, 0, 0, 8, ch195data);
		// char: 0xc2 */

		static byte[] ch194data = {
			0xee,
			0x44,
			0x7c,
			0x28,
			0x28,
			0x38,
			0x10,
			0x0,
			0x28,
			0x10

		};

		static BitmapCharRec ch194 = new BitmapCharRec(7, 10, 0, 0, 8, ch194data);
		// char: 0xc1 */

		static byte[] ch193data = {
			0xee,
			0x44,
			0x7c,
			0x28,
			0x28,
			0x38,
			0x10,
			0x0,
			0x10,
			0x8

		};

		static BitmapCharRec ch193 = new BitmapCharRec(7, 10, 0, 0, 8, ch193data);
		// char: 0xc0 */

		static byte[] ch192data = {
			0xee,
			0x44,
			0x7c,
			0x28,
			0x28,
			0x38,
			0x10,
			0x0,
			0x10,
			0x20

		};

		static BitmapCharRec ch192 = new BitmapCharRec(7, 10, 0, 0, 8, ch192data);
		// char: 0xbf */

		static byte[] ch191data = {
			0xe0,
			0xa0,
			0x80,
			0x40,
			0x40,
			0x0,
			0x40

		};

		static BitmapCharRec ch191 = new BitmapCharRec(3, 7, 0, 2, 4, ch191data);
		// char: 0xbe */

		static byte[] ch190data = {
			0x44,
			0x3e,
			0x2c,
			0xd4,
			0x28,
			0x48,
			0xe4

		};

		static BitmapCharRec ch190 = new BitmapCharRec(7, 7, 0, 0, 8, ch190data);
		// char: 0xbd */

		static byte[] ch189data = {
			0x4e,
			0x24,
			0x2a,
			0xf6,
			0x48,
			0xc8,
			0x44

		};

		static BitmapCharRec ch189 = new BitmapCharRec(7, 7, 0, 0, 8, ch189data);
		// char: 0xbc */

		static byte[] ch188data = {
			0x44,
			0x3e,
			0x2c,
			0xf4,
			0x48,
			0xc8,
			0x44

		};

		static BitmapCharRec ch188 = new BitmapCharRec(7, 7, 0, 0, 8, ch188data);
		// char: 0xbb */

		static byte[] ch187data = {
			0xa0,
			0x50,
			0x50,
			0xa0

		};

		static BitmapCharRec ch187 = new BitmapCharRec(4, 4, 0, -1, 5, ch187data);
		// char: 0xba */

		static byte[] ch186data = {
			0xe0,
			0x0,
			0x40,
			0xa0,
			0x40

		};

		static BitmapCharRec ch186 = new BitmapCharRec(3, 5, 0, -2, 4, ch186data);
		// char: 0xb9 */

		static byte[] ch185data = {
			0xe0,
			0x40,
			0xc0,
			0x40

		};

		static BitmapCharRec ch185 = new BitmapCharRec(3, 4, 0, -3, 3, ch185data);
		// char: 0xb8 */

		static byte[] ch184data = {
			0xc0,
			0x20,
			0x40

		};

		static BitmapCharRec ch184 = new BitmapCharRec(3, 3, 0, 3, 4, ch184data);
		// char: 0xb7 */


		static byte[] ch183data = { 0x80 };

		static BitmapCharRec ch183 = new BitmapCharRec(1, 1, 0, -2, 2, ch183data);
		// char: 0xb6 */

		static byte[] ch182data = {
			0x28,
			0x28,
			0x28,
			0x28,
			0x68,
			0xe8,
			0xe8,
			0xe8,
			0x7c

		};

		static BitmapCharRec ch182 = new BitmapCharRec(6, 9, 0, 2, 6, ch182data);
		// char: 0xb5 */

		static byte[] ch181data = {
			0x80,
			0x80,
			0xe8,
			0x90,
			0x90,
			0x90,
			0x90

		};

		static BitmapCharRec ch181 = new BitmapCharRec(5, 7, 0, 2, 5, ch181data);
		// char: 0xb4 */

		static byte[] ch180data = {
			0x80,
			0x40

		};

		static BitmapCharRec ch180 = new BitmapCharRec(2, 2, 0, -5, 3, ch180data);
		// char: 0xb3 */

		static byte[] ch179data = {
			0xc0,
			0x20,
			0x40,
			0xe0

		};

		static BitmapCharRec ch179 = new BitmapCharRec(3, 4, 0, -3, 3, ch179data);
		// char: 0xb2 */

		static byte[] ch178data = {
			0xe0,
			0x40,
			0xa0,
			0x60

		};

		static BitmapCharRec ch178 = new BitmapCharRec(3, 4, 0, -3, 3, ch178data);
		// char: 0xb1 */

		static byte[] ch177data = {
			0xf8,
			0x0,
			0x20,
			0x20,
			0xf8,
			0x20,
			0x20

		};

		static BitmapCharRec ch177 = new BitmapCharRec(5, 7, 0, 0, 6, ch177data);
		// char: 0xb0 */

		static byte[] ch176data = {
			0x60,
			0x90,
			0x90,
			0x60

		};

		static BitmapCharRec ch176 = new BitmapCharRec(4, 4, 0, -3, 4, ch176data);
		// char: 0xaf */


		static byte[] ch175data = { 0xe0 };

		static BitmapCharRec ch175 = new BitmapCharRec(3, 1, 0, -6, 4, ch175data);
		// char: 0xae */

		static byte[] ch174data = {
			0x38,
			0x44,
			0xaa,
			0xb2,
			0xba,
			0x44,
			0x38

		};

		static BitmapCharRec ch174 = new BitmapCharRec(7, 7, -1, 0, 9, ch174data);
		// char: 0xad */


		static byte[] ch173data = { 0xe0 };

		static BitmapCharRec ch173 = new BitmapCharRec(3, 1, 0, -2, 4, ch173data);
		// char: 0xac */

		static byte[] ch172data = {
			0x8,
			0x8,
			0xf8

		};

		static BitmapCharRec ch172 = new BitmapCharRec(5, 3, -1, -1, 7, ch172data);
		// char: 0xab */

		static byte[] ch171data = {
			0x50,
			0xa0,
			0xa0,
			0x50

		};

		static BitmapCharRec ch171 = new BitmapCharRec(4, 4, 0, -1, 5, ch171data);
		// char: 0xaa */

		static byte[] ch170data = {
			0xe0,
			0x0,
			0xa0,
			0x20,
			0xc0

		};

		static BitmapCharRec ch170 = new BitmapCharRec(3, 5, 0, -2, 4, ch170data);
		// char: 0xa9 */

		static byte[] ch169data = {
			0x38,
			0x44,
			0x9a,
			0xa2,
			0x9a,
			0x44,
			0x38

		};

		static BitmapCharRec ch169 = new BitmapCharRec(7, 7, -1, 0, 9, ch169data);
		// char: 0xa8 */


		static byte[] ch168data = { 0xa0 };

		static BitmapCharRec ch168 = new BitmapCharRec(3, 1, -1, -6, 5, ch168data);
		// char: 0xa7 */

		static byte[] ch167data = {
			0xe0,
			0x90,
			0x20,
			0x50,
			0x90,
			0xa0,
			0x40,
			0x90,
			0x70

		};

		static BitmapCharRec ch167 = new BitmapCharRec(4, 9, 0, 1, 5, ch167data);
		// char: 0xa6 */

		static byte[] ch166data = {
			0x80,
			0x80,
			0x80,
			0x0,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch166 = new BitmapCharRec(1, 7, 0, 0, 2, ch166data);
		// char: 0xa5 */

		static byte[] ch165data = {
			0x70,
			0x20,
			0xf8,
			0x20,
			0xd8,
			0x50,
			0x88

		};

		static BitmapCharRec ch165 = new BitmapCharRec(5, 7, 0, 0, 5, ch165data);
		// char: 0xa4 */

		static byte[] ch164data = {
			0x88,
			0x70,
			0x50,
			0x50,
			0x70,
			0x88

		};

		static BitmapCharRec ch164 = new BitmapCharRec(5, 6, 0, -1, 5, ch164data);
		// char: 0xa3 */

		static byte[] ch163data = {
			0xf0,
			0xc8,
			0x40,
			0xe0,
			0x40,
			0x50,
			0x30

		};

		static BitmapCharRec ch163 = new BitmapCharRec(5, 7, 0, 0, 5, ch163data);
		// char: 0xa2 */

		static byte[] ch162data = {
			0x80,
			0xe0,
			0x90,
			0x80,
			0x90,
			0x70,
			0x10

		};

		static BitmapCharRec ch162 = new BitmapCharRec(4, 7, 0, 1, 5, ch162data);
		// char: 0xa1 */

		static byte[] ch161data = {
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x0,
			0x80

		};

		static BitmapCharRec ch161 = new BitmapCharRec(1, 7, -1, 2, 3, ch161data);
		// char: 0xa0 */


		static BitmapCharRec ch160 = new BitmapCharRec(0, 0, 0, 0, 2, null);
		// char: 0x7e '~' */

		static byte[] ch126data = {
			0x98,
			0x64

		};

		static BitmapCharRec ch126 = new BitmapCharRec(6, 2, 0, -2, 7, ch126data);
		// char: 0x7d '}' */

		static byte[] ch125data = {
			0x80,
			0x40,
			0x40,
			0x40,
			0x20,
			0x40,
			0x40,
			0x40,
			0x80

		};

		static BitmapCharRec ch125 = new BitmapCharRec(3, 9, 0, 2, 4, ch125data);
		// char: 0x7c '|' */

		static byte[] ch124data = {
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

		static BitmapCharRec ch124 = new BitmapCharRec(1, 9, 0, 2, 2, ch124data);
		// char: 0x7b '{' */

		static byte[] ch123data = {
			0x20,
			0x40,
			0x40,
			0x40,
			0x80,
			0x40,
			0x40,
			0x40,
			0x20

		};

		static BitmapCharRec ch123 = new BitmapCharRec(3, 9, 0, 2, 4, ch123data);
		// char: 0x7a 'z' */

		static byte[] ch122data = {
			0xf0,
			0x90,
			0x40,
			0x20,
			0xf0

		};

		static BitmapCharRec ch122 = new BitmapCharRec(4, 5, 0, 0, 5, ch122data);
		// char: 0x79 'y' */

		static byte[] ch121data = {
			0x40,
			0x40,
			0x20,
			0x30,
			0x50,
			0x48,
			0xdc

		};

		static BitmapCharRec ch121 = new BitmapCharRec(6, 7, 1, 2, 5, ch121data);
		// char: 0x78 'x' */

		static byte[] ch120data = {
			0xd8,
			0x50,
			0x20,
			0x50,
			0xd8

		};

		static BitmapCharRec ch120 = new BitmapCharRec(5, 5, 0, 0, 6, ch120data);
		// char: 0x77 'w' */

		static byte[] ch119data = {
			0x28,
			0x6c,
			0x54,
			0x92,
			0xdb

		};

		static BitmapCharRec ch119 = new BitmapCharRec(8, 5, 0, 0, 8, ch119data);
		// char: 0x76 'v' */

		static byte[] ch118data = {
			0x20,
			0x60,
			0x50,
			0x90,
			0xd8

		};

		static BitmapCharRec ch118 = new BitmapCharRec(5, 5, 0, 0, 5, ch118data);
		// char: 0x75 'u' */

		static byte[] ch117data = {
			0x68,
			0x90,
			0x90,
			0x90,
			0x90

		};

		static BitmapCharRec ch117 = new BitmapCharRec(5, 5, 0, 0, 5, ch117data);
		// char: 0x74 't' */

		static byte[] ch116data = {
			0x30,
			0x40,
			0x40,
			0x40,
			0xe0,
			0x40

		};

		static BitmapCharRec ch116 = new BitmapCharRec(4, 6, 0, 0, 4, ch116data);
		// char: 0x73 's' */

		static byte[] ch115data = {
			0xe0,
			0x20,
			0x60,
			0x80,
			0xe0

		};

		static BitmapCharRec ch115 = new BitmapCharRec(3, 5, 0, 0, 4, ch115data);
		// char: 0x72 'r' */

		static byte[] ch114data = {
			0xe0,
			0x40,
			0x40,
			0x60,
			0xa0

		};

		static BitmapCharRec ch114 = new BitmapCharRec(3, 5, 0, 0, 4, ch114data);
		// char: 0x71 'q' */

		static byte[] ch113data = {
			0x38,
			0x10,
			0x70,
			0x90,
			0x90,
			0x90,
			0x70

		};

		static BitmapCharRec ch113 = new BitmapCharRec(5, 7, 0, 2, 5, ch113data);
		// char: 0x70 'p' */

		static byte[] ch112data = {
			0xc0,
			0x80,
			0xe0,
			0x90,
			0x90,
			0x90,
			0xe0

		};

		static BitmapCharRec ch112 = new BitmapCharRec(4, 7, 0, 2, 5, ch112data);
		// char: 0x6f 'o' */

		static byte[] ch111data = {
			0x60,
			0x90,
			0x90,
			0x90,
			0x60

		};

		static BitmapCharRec ch111 = new BitmapCharRec(4, 5, 0, 0, 5, ch111data);
		// char: 0x6e 'n' */

		static byte[] ch110data = {
			0xd8,
			0x90,
			0x90,
			0x90,
			0xe0

		};

		static BitmapCharRec ch110 = new BitmapCharRec(5, 5, 0, 0, 5, ch110data);
		// char: 0x6d 'm' */

		static byte[] ch109data = {
			0xdb,
			0x92,
			0x92,
			0x92,
			0xec

		};

		static BitmapCharRec ch109 = new BitmapCharRec(8, 5, 0, 0, 8, ch109data);
		// char: 0x6c 'l' */

		static byte[] ch108data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0x40,
			0x40,
			0xc0

		};

		static BitmapCharRec ch108 = new BitmapCharRec(3, 7, 0, 0, 4, ch108data);
		// char: 0x6b 'k' */

		static byte[] ch107data = {
			0x98,
			0x90,
			0xe0,
			0xa0,
			0x90,
			0x80,
			0x80

		};

		static BitmapCharRec ch107 = new BitmapCharRec(5, 7, 0, 0, 5, ch107data);
		// char: 0x6a 'j' */

		static byte[] ch106data = {
			0x80,
			0x40,
			0x40,
			0x40,
			0x40,
			0x40,
			0xc0,
			0x0,
			0x40

		};

		static BitmapCharRec ch106 = new BitmapCharRec(2, 9, 0, 2, 3, ch106data);
		// char: 0x69 'i' */

		static byte[] ch105data = {
			0x40,
			0x40,
			0x40,
			0x40,
			0xc0,
			0x0,
			0x40

		};

		static BitmapCharRec ch105 = new BitmapCharRec(2, 7, 0, 0, 3, ch105data);
		// char: 0x68 'h' */

		static byte[] ch104data = {
			0xd8,
			0x90,
			0x90,
			0x90,
			0xe0,
			0x80,
			0x80

		};

		static BitmapCharRec ch104 = new BitmapCharRec(5, 7, 0, 0, 5, ch104data);
		// char: 0x67 'g' */

		static byte[] ch103data = {
			0xe0,
			0x90,
			0x60,
			0x40,
			0xa0,
			0xa0,
			0x70

		};

		static BitmapCharRec ch103 = new BitmapCharRec(4, 7, 0, 2, 5, ch103data);
		// char: 0x66 'f' */

		static byte[] ch102data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0xe0,
			0x40,
			0x30

		};

		static BitmapCharRec ch102 = new BitmapCharRec(4, 7, 0, 0, 4, ch102data);
		// char: 0x65 'e' */

		static byte[] ch101data = {
			0x60,
			0x80,
			0xc0,
			0xa0,
			0x60

		};

		static BitmapCharRec ch101 = new BitmapCharRec(3, 5, 0, 0, 4, ch101data);
		// char: 0x64 'd' */

		static byte[] ch100data = {
			0x68,
			0x90,
			0x90,
			0x90,
			0x70,
			0x10,
			0x30

		};

		static BitmapCharRec ch100 = new BitmapCharRec(5, 7, 0, 0, 5, ch100data);
		// char: 0x63 'c' */

		static byte[] ch99data = {
			0x60,
			0x80,
			0x80,
			0x80,
			0x60

		};

		static BitmapCharRec ch99 = new BitmapCharRec(3, 5, 0, 0, 4, ch99data);
		// char: 0x62 'b' */

		static byte[] ch98data = {
			0xe0,
			0x90,
			0x90,
			0x90,
			0xe0,
			0x80,
			0x80

		};

		static BitmapCharRec ch98 = new BitmapCharRec(4, 7, 0, 0, 5, ch98data);
		// char: 0x61 'a' */

		static byte[] ch97data = {
			0xe0,
			0xa0,
			0x60,
			0x20,
			0xc0

		};

		static BitmapCharRec ch97 = new BitmapCharRec(3, 5, 0, 0, 4, ch97data);
		// char: 0x60 '`' */

		static byte[] ch96data = {
			0xc0,
			0x80

		};

		static BitmapCharRec ch96 = new BitmapCharRec(2, 2, 0, -5, 3, ch96data);
		// char: 0x5f '_' */


		static byte[] ch95data = { 0xf8 };

		static BitmapCharRec ch95 = new BitmapCharRec(5, 1, 0, 3, 5, ch95data);
		// char: 0x5e '^' */

		static byte[] ch94data = {
			0xa0,
			0xa0,
			0x40

		};

		static BitmapCharRec ch94 = new BitmapCharRec(3, 3, -1, -4, 5, ch94data);
		// char: 0x5d ']' */

		static byte[] ch93data = {
			0xc0,
			0x40,
			0x40,
			0x40,
			0x40,
			0x40,
			0x40,
			0x40,
			0xc0

		};

		static BitmapCharRec ch93 = new BitmapCharRec(2, 9, 0, 2, 3, ch93data);
		// char: 0x5c '\' */

		static byte[] ch92data = {
			0x20,
			0x20,
			0x40,
			0x40,
			0x40,
			0x80,
			0x80

		};

		static BitmapCharRec ch92 = new BitmapCharRec(3, 7, 0, 0, 3, ch92data);
		// char: 0x5b '[' */

		static byte[] ch91data = {
			0xc0,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80,
			0xc0

		};

		static BitmapCharRec ch91 = new BitmapCharRec(2, 9, 0, 2, 3, ch91data);
		// char: 0x5a 'Z' */

		static byte[] ch90data = {
			0xf8,
			0x88,
			0x40,
			0x20,
			0x10,
			0x88,
			0xf8

		};

		static BitmapCharRec ch90 = new BitmapCharRec(5, 7, 0, 0, 6, ch90data);
		// char: 0x59 'Y' */

		static byte[] ch89data = {
			0x38,
			0x10,
			0x10,
			0x28,
			0x28,
			0x44,
			0xee

		};

		static BitmapCharRec ch89 = new BitmapCharRec(7, 7, 0, 0, 8, ch89data);
		// char: 0x58 'X' */

		static byte[] ch88data = {
			0xee,
			0x44,
			0x28,
			0x10,
			0x28,
			0x44,
			0xee

		};

		static BitmapCharRec ch88 = new BitmapCharRec(7, 7, 0, 0, 8, ch88data);
		// char: 0x57 'W' */

		static byte[] ch87data = {
			0x22,
			0x0,
			0x22,
			0x0,
			0x55,
			0x0,
			0x55,
			0x0,
			0xc9,
			0x80,
			0x88,
			0x80,
			0xdd,
			0xc0

		};

		static BitmapCharRec ch87 = new BitmapCharRec(10, 7, 0, 0, 10, ch87data);
		// char: 0x56 'V' */

		static byte[] ch86data = {
			0x10,
			0x10,
			0x28,
			0x28,
			0x6c,
			0x44,
			0xee

		};

		static BitmapCharRec ch86 = new BitmapCharRec(7, 7, 0, 0, 8, ch86data);
		// char: 0x55 'U' */

		static byte[] ch85data = {
			0x38,
			0x6c,
			0x44,
			0x44,
			0x44,
			0x44,
			0xee

		};

		static BitmapCharRec ch85 = new BitmapCharRec(7, 7, 0, 0, 8, ch85data);
		// char: 0x54 'T' */

		static byte[] ch84data = {
			0x70,
			0x20,
			0x20,
			0x20,
			0x20,
			0xa8,
			0xf8

		};

		static BitmapCharRec ch84 = new BitmapCharRec(5, 7, 0, 0, 6, ch84data);
		// char: 0x53 'S' */

		static byte[] ch83data = {
			0xe0,
			0x90,
			0x10,
			0x60,
			0xc0,
			0x90,
			0x70

		};

		static BitmapCharRec ch83 = new BitmapCharRec(4, 7, 0, 0, 5, ch83data);
		// char: 0x52 'R' */

		static byte[] ch82data = {
			0xec,
			0x48,
			0x50,
			0x70,
			0x48,
			0x48,
			0xf0

		};

		static BitmapCharRec ch82 = new BitmapCharRec(6, 7, 0, 0, 7, ch82data);
		// char: 0x51 'Q' */

		static byte[] ch81data = {
			0xc,
			0x18,
			0x70,
			0xcc,
			0x84,
			0x84,
			0x84,
			0xcc,
			0x78

		};

		static BitmapCharRec ch81 = new BitmapCharRec(6, 9, 0, 2, 7, ch81data);
		// char: 0x50 'P' */

		static byte[] ch80data = {
			0xe0,
			0x40,
			0x40,
			0x70,
			0x48,
			0x48,
			0xf0

		};

		static BitmapCharRec ch80 = new BitmapCharRec(5, 7, 0, 0, 6, ch80data);
		// char: 0x4f 'O' */

		static byte[] ch79data = {
			0x78,
			0xcc,
			0x84,
			0x84,
			0x84,
			0xcc,
			0x78

		};

		static BitmapCharRec ch79 = new BitmapCharRec(6, 7, 0, 0, 7, ch79data);
		// char: 0x4e 'N' */

		static byte[] ch78data = {
			0xe4,
			0x4c,
			0x4c,
			0x54,
			0x54,
			0x64,
			0xee

		};

		static BitmapCharRec ch78 = new BitmapCharRec(7, 7, 0, 0, 8, ch78data);
		// char: 0x4d 'M' */

		static byte[] ch77data = {
			0xeb,
			0x80,
			0x49,
			0x0,
			0x55,
			0x0,
			0x55,
			0x0,
			0x63,
			0x0,
			0x63,
			0x0,
			0xe3,
			0x80

		};

		static BitmapCharRec ch77 = new BitmapCharRec(9, 7, 0, 0, 10, ch77data);
		// char: 0x4c 'L' */

		static byte[] ch76data = {
			0xf8,
			0x48,
			0x40,
			0x40,
			0x40,
			0x40,
			0xe0

		};

		static BitmapCharRec ch76 = new BitmapCharRec(5, 7, 0, 0, 6, ch76data);
		// char: 0x4b 'K' */

		static byte[] ch75data = {
			0xec,
			0x48,
			0x50,
			0x60,
			0x50,
			0x48,
			0xec

		};

		static BitmapCharRec ch75 = new BitmapCharRec(6, 7, 0, 0, 7, ch75data);
		// char: 0x4a 'J' */

		static byte[] ch74data = {
			0xc0,
			0xa0,
			0x20,
			0x20,
			0x20,
			0x20,
			0x70

		};

		static BitmapCharRec ch74 = new BitmapCharRec(4, 7, 0, 0, 4, ch74data);
		// char: 0x49 'I' */

		static byte[] ch73data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0x40,
			0x40,
			0xe0

		};

		static BitmapCharRec ch73 = new BitmapCharRec(3, 7, 0, 0, 4, ch73data);
		// char: 0x48 'H' */

		static byte[] ch72data = {
			0xee,
			0x44,
			0x44,
			0x7c,
			0x44,
			0x44,
			0xee

		};

		static BitmapCharRec ch72 = new BitmapCharRec(7, 7, 0, 0, 8, ch72data);
		// char: 0x47 'G' */

		static byte[] ch71data = {
			0x78,
			0xc4,
			0x84,
			0x9c,
			0x80,
			0xc4,
			0x7c

		};

		static BitmapCharRec ch71 = new BitmapCharRec(6, 7, 0, 0, 7, ch71data);
		// char: 0x46 'F' */

		static byte[] ch70data = {
			0xe0,
			0x40,
			0x40,
			0x70,
			0x40,
			0x48,
			0xf8

		};

		static BitmapCharRec ch70 = new BitmapCharRec(5, 7, 0, 0, 6, ch70data);
		// char: 0x45 'E' */

		static byte[] ch69data = {
			0xf8,
			0x48,
			0x40,
			0x70,
			0x40,
			0x48,
			0xf8

		};

		static BitmapCharRec ch69 = new BitmapCharRec(5, 7, 0, 0, 6, ch69data);
		// char: 0x44 'D' */

		static byte[] ch68data = {
			0xf8,
			0x4c,
			0x44,
			0x44,
			0x44,
			0x4c,
			0xf8

		};

		static BitmapCharRec ch68 = new BitmapCharRec(6, 7, 0, 0, 7, ch68data);
		// char: 0x43 'C' */

		static byte[] ch67data = {
			0x78,
			0xc4,
			0x80,
			0x80,
			0x80,
			0xc4,
			0x7c

		};

		static BitmapCharRec ch67 = new BitmapCharRec(6, 7, 0, 0, 7, ch67data);
		// char: 0x42 'B' */

		static byte[] ch66data = {
			0xf0,
			0x48,
			0x48,
			0x70,
			0x48,
			0x48,
			0xf0

		};

		static BitmapCharRec ch66 = new BitmapCharRec(5, 7, 0, 0, 6, ch66data);
		// char: 0x41 'A' */

		static byte[] ch65data = {
			0xee,
			0x44,
			0x7c,
			0x28,
			0x28,
			0x38,
			0x10

		};

		static BitmapCharRec ch65 = new BitmapCharRec(7, 7, 0, 0, 8, ch65data);
		// char: 0x40 '@' */

		static byte[] ch64data = {
			0x3e,
			0x40,
			0x92,
			0xad,
			0xa5,
			0xa5,
			0x9d,
			0x42,
			0x3c

		};

		static BitmapCharRec ch64 = new BitmapCharRec(8, 9, 0, 2, 9, ch64data);
		// char: 0x3f '?' */

		static byte[] ch63data = {
			0x40,
			0x0,
			0x40,
			0x40,
			0x20,
			0xa0,
			0xe0

		};

		static BitmapCharRec ch63 = new BitmapCharRec(3, 7, 0, 0, 4, ch63data);
		// char: 0x3e '>' */

		static byte[] ch62data = {
			0x80,
			0x40,
			0x20,
			0x40,
			0x80

		};

		static BitmapCharRec ch62 = new BitmapCharRec(3, 5, 0, 0, 5, ch62data);
		// char: 0x3d '=' */

		static byte[] ch61data = {
			0xf8,
			0x0,
			0xf8

		};

		static BitmapCharRec ch61 = new BitmapCharRec(5, 3, 0, -1, 6, ch61data);
		// char: 0x3c '<' */

		static byte[] ch60data = {
			0x20,
			0x40,
			0x80,
			0x40,
			0x20

		};

		static BitmapCharRec ch60 = new BitmapCharRec(3, 5, -1, 0, 5, ch60data);
		// char: 0x3b ';' */

		static byte[] ch59data = {
			0x80,
			0x80,
			0x80,
			0x0,
			0x0,
			0x0,
			0x80

		};

		static BitmapCharRec ch59 = new BitmapCharRec(1, 7, -1, 2, 3, ch59data);
		// char: 0x3a ':' */

		static byte[] ch58data = {
			0x80,
			0x0,
			0x0,
			0x0,
			0x80

		};

		static BitmapCharRec ch58 = new BitmapCharRec(1, 5, -1, 0, 3, ch58data);
		// char: 0x39 '9' */

		static byte[] ch57data = {
			0xc0,
			0x20,
			0x70,
			0x90,
			0x90,
			0x90,
			0x60

		};

		static BitmapCharRec ch57 = new BitmapCharRec(4, 7, 0, 0, 5, ch57data);
		// char: 0x38 '8' */

		static byte[] ch56data = {
			0x60,
			0x90,
			0x90,
			0x60,
			0x90,
			0x90,
			0x60

		};

		static BitmapCharRec ch56 = new BitmapCharRec(4, 7, 0, 0, 5, ch56data);
		// char: 0x37 '7' */

		static byte[] ch55data = {
			0x40,
			0x40,
			0x40,
			0x20,
			0x20,
			0x90,
			0xf0

		};

		static BitmapCharRec ch55 = new BitmapCharRec(4, 7, 0, 0, 5, ch55data);
		// char: 0x36 '6' */

		static byte[] ch54data = {
			0x60,
			0x90,
			0x90,
			0x90,
			0xe0,
			0x40,
			0x30

		};

		static BitmapCharRec ch54 = new BitmapCharRec(4, 7, 0, 0, 5, ch54data);
		// char: 0x35 '5' */

		static byte[] ch53data = {
			0xe0,
			0x90,
			0x10,
			0x10,
			0xe0,
			0x40,
			0x70

		};

		static BitmapCharRec ch53 = new BitmapCharRec(4, 7, 0, 0, 5, ch53data);
		// char: 0x34 '4' */

		static byte[] ch52data = {
			0x10,
			0x10,
			0xf8,
			0x90,
			0x50,
			0x30,
			0x10

		};

		static BitmapCharRec ch52 = new BitmapCharRec(5, 7, 0, 0, 5, ch52data);
		// char: 0x33 '3' */

		static byte[] ch51data = {
			0xe0,
			0x10,
			0x10,
			0x60,
			0x10,
			0x90,
			0x60

		};

		static BitmapCharRec ch51 = new BitmapCharRec(4, 7, 0, 0, 5, ch51data);
		// char: 0x32 '2' */

		static byte[] ch50data = {
			0xf0,
			0x40,
			0x20,
			0x20,
			0x10,
			0x90,
			0x60

		};

		static BitmapCharRec ch50 = new BitmapCharRec(4, 7, 0, 0, 5, ch50data);
		// char: 0x31 '1' */

		static byte[] ch49data = {
			0xe0,
			0x40,
			0x40,
			0x40,
			0x40,
			0xc0,
			0x40

		};

		static BitmapCharRec ch49 = new BitmapCharRec(3, 7, -1, 0, 5, ch49data);
		// char: 0x30 '0' */

		static byte[] ch48data = {
			0x60,
			0x90,
			0x90,
			0x90,
			0x90,
			0x90,
			0x60

		};

		static BitmapCharRec ch48 = new BitmapCharRec(4, 7, 0, 0, 5, ch48data);
		// char: 0x2f '/' */

		static byte[] ch47data = {
			0x80,
			0x80,
			0x40,
			0x40,
			0x40,
			0x20,
			0x20

		};

		static BitmapCharRec ch47 = new BitmapCharRec(3, 7, 0, 0, 3, ch47data);
		// char: 0x2e '.' */


		static byte[] ch46data = { 0x80 };

		static BitmapCharRec ch46 = new BitmapCharRec(1, 1, -1, 0, 3, ch46data);
		// char: 0x2d '-' */


		static byte[] ch45data = { 0xf0 };

		static BitmapCharRec ch45 = new BitmapCharRec(4, 1, -1, -2, 7, ch45data);
		// char: 0x2c ',' */

		static byte[] ch44data = {
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch44 = new BitmapCharRec(1, 3, -1, 2, 3, ch44data);
		// char: 0x2b '+' */

		static byte[] ch43data = {
			0x20,
			0x20,
			0xf8,
			0x20,
			0x20

		};

		static BitmapCharRec ch43 = new BitmapCharRec(5, 5, 0, 0, 6, ch43data);
		// char: 0x2a '*' */

		static byte[] ch42data = {
			0xa0,
			0x40,
			0xa0

		};

		static BitmapCharRec ch42 = new BitmapCharRec(3, 3, 0, -4, 5, ch42data);
		// char: 0x29 ')' */

		static byte[] ch41data = {
			0x80,
			0x40,
			0x40,
			0x20,
			0x20,
			0x20,
			0x40,
			0x40,
			0x80

		};

		static BitmapCharRec ch41 = new BitmapCharRec(3, 9, 0, 2, 4, ch41data);
		// char: 0x28 '(' */

		static byte[] ch40data = {
			0x20,
			0x40,
			0x40,
			0x80,
			0x80,
			0x80,
			0x40,
			0x40,
			0x20

		};

		static BitmapCharRec ch40 = new BitmapCharRec(3, 9, 0, 2, 4, ch40data);
		// char: 0x27 ''' */

		static byte[] ch39data = {
			0x40,
			0xc0

		};

		static BitmapCharRec ch39 = new BitmapCharRec(2, 2, 0, -5, 3, ch39data);
		// char: 0x26 '&' */

		static byte[] ch38data = {
			0x76,
			0x8d,
			0x98,
			0x74,
			0x6e,
			0x50,
			0x30

		};

		static BitmapCharRec ch38 = new BitmapCharRec(8, 7, 0, 0, 8, ch38data);
		// char: 0x25 '%' */

		static byte[] ch37data = {
			0x44,
			0x2a,
			0x2a,
			0x56,
			0xa8,
			0xa4,
			0x7e

		};

		static BitmapCharRec ch37 = new BitmapCharRec(7, 7, 0, 0, 8, ch37data);
		// char: 0x24 '$' */

		static byte[] ch36data = {
			0x20,
			0xe0,
			0x90,
			0x10,
			0x60,
			0x80,
			0x90,
			0x70,
			0x20

		};

		static BitmapCharRec ch36 = new BitmapCharRec(4, 9, 0, 1, 5, ch36data);
		// char: 0x23 '#' */

		static byte[] ch35data = {
			0x50,
			0x50,
			0xf8,
			0x50,
			0xf8,
			0x50,
			0x50

		};

		static BitmapCharRec ch35 = new BitmapCharRec(5, 7, 0, 0, 5, ch35data);
		// char: 0x22 '"' */

		static byte[] ch34data = {
			0xa0,
			0xa0

		};

		static BitmapCharRec ch34 = new BitmapCharRec(3, 2, 0, -5, 4, ch34data);
		// char: 0x21 '!' */

		static byte[] ch33data = {
			0x80,
			0x0,
			0x80,
			0x80,
			0x80,
			0x80,
			0x80

		};

		static BitmapCharRec ch33 = new BitmapCharRec(1, 7, -1, 0, 3, ch33data);
		// char: 0x20 ' ' */


		static BitmapCharRec ch32 = new BitmapCharRec(0, 0, 0, 0, 2, null);
		static BitmapCharRec[] chars = {
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
		public static BitmapFontRec glutBitmapTimesRoman10 = new BitmapFontRec("-adobe-times-medium-r-normal--10-100-75-75-p-54-iso8859-1", 224, 32, chars);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
