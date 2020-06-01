/**
 * Copyright (c) 2017 The Campfire Union Inc - All Rights Reserved.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for
 * full license information.
 *
 * Email:   info@campfireunion.com
 * Website: https://www.campfireunion.com
 */

namespace VRKeys.Layouts {

	/// <summary>
	/// French language keyboard (AZERTY). Note that some liberties had to be taken
	/// because of the lack of shift + ctrl combination and diacritic key.
	/// </summary>
	public class French : Layout {

		public French () {
			placeholderMessage = "입력...";

			spaceButtonLabel = "SPACE";

			enterButtonLabel = "ENTER";

			cancelButtonLabel = "CANCEL";

			shiftButtonLabel = "SHIFT";

			backspaceButtonLabel = "BACKSPACE";

			clearButtonLabel = "CLEAR";
            
            row1Keys = new string[] { "`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=" };

            row1Shift = new string[] { "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+" };

            row2Keys = new string[] { "ㅂ", "ㅈ", "ㄷ", "ㄱ", "ㅅ", "ㅛ", "ㅕ", "ㅑ", "ㅐ", "ㅔ", "[", "]", "\\" };

            row2Shift = new string[] { "ㅃ", "ㅉ", "ㄸ", "ㄲ", "ㅆ", "Y", "U", "I", "ㅒ", "ㅖ", "{", "}", "|" };

            row3Keys = new string[] { "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'" };

            row3Shift = new string[] { "A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\"" };

            row4Keys = new string[] { "z", "x", "c", "v", "b", "n", "m", ",", ".", "?" };

            row4Shift = new string[] { "Z", "X", "C", "V", "B", "N", "M", "<", ">", "/" };

            row1Offset = 0.16f;

			row2Offset = 0.08f;

			row3Offset = 0f;

			row4Offset = 0f;
		}
	}
}