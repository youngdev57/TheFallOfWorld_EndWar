/**
 * Copyright (c) 2017 The Campfire Union Inc - All Rights Reserved.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for
 * full license information.
 *
 * Email:   info@campfireunion.com
 * Website: https://www.campfireunion.com
 */

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.Collections;

namespace VRKeys {

	/// <summary>
	/// Example use of VRKeys keyboard.
	/// </summary>
	public class VRKeyManager : MonoBehaviour {

		/// <summary>
		/// Reference to the VRKeys keyboard.
		/// </summary>
		public Keyboard keyboard;

        public PhotonTest photonManager;

        public Camera cam;

        public enum Status
        {
            email,
            password,
            nickname
        }

        public TextMeshProUGUI inputLabel;

        public Status status = Status.email;

        string email = "";
        string password = "";

		/// <summary>
		/// See the following for why this is so convoluted:
		/// http://referencesource.microsoft.com/#System.ComponentModel.DataAnnotations/DataAnnotations/EmailAddressAttribute.cs,54
		/// http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx/
		/// </summary>
		private Regex emailValidator = new Regex (@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.IgnoreCase);

		/// <summary>
		/// Show the keyboard with a custom input message. Attaching events dynamically,
		/// but you can also use the inspector.
		/// </summary>
		private void OnEnable () {
			// Automatically creating camera here to show how
			//GameObject camera = new GameObject ("Main Camera");
			//Camera cam = camera.AddComponent<Camera> ();
			//cam.nearClipPlane = 0.1f;
			//camera.AddComponent<AudioListener> ();

			// Improves event system performance
			Canvas canvas = keyboard.canvas.GetComponent<Canvas> ();
			canvas.worldCamera = cam;

			keyboard.Enable ();
			keyboard.SetPlaceholderMessage ("입력...");

			keyboard.OnUpdate.AddListener (HandleUpdate);
			keyboard.OnSubmit.AddListener (HandleSubmit);
			keyboard.OnCancel.AddListener (HandleCancel);
        }

		private void OnDisable () {
			keyboard.OnUpdate.RemoveListener (HandleUpdate);
			keyboard.OnSubmit.RemoveListener (HandleSubmit);
			keyboard.OnCancel.RemoveListener (HandleCancel);

			keyboard.Disable ();
		}

		/// <summary>
		/// Press space to show/hide the keyboard.
		///
		/// Press Q for Qwerty keyboard, D for Dvorak keyboard, and F for French keyboard.
		/// </summary>
		private void Update () {
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (keyboard.disabled) {
					keyboard.Enable ();
				} else {
					keyboard.Disable ();
				}
			}

			if (keyboard.disabled) {
				return;
			}

			if (Input.GetKeyDown (KeyCode.Q)) {
				keyboard.SetLayout (KeyboardLayout.Qwerty);
			} else if (Input.GetKeyDown (KeyCode.F)) {
				keyboard.SetLayout (KeyboardLayout.French);
			} else if (Input.GetKeyDown (KeyCode.D)) {
				keyboard.SetLayout (KeyboardLayout.Dvorak);
			}

            if(Input.GetKeyDown(KeyCode.Y))     //로그인 테스트용 ㅡㅡ
            {
                photonManager.OnClickLogin("1", "1");
                StartCoroutine(CheckLoginStatus());
            }

            if (Input.GetKeyDown(KeyCode.T))     //로그인 테스트용 ㅡㅡ
            {
                photonManager.OnClickLogin("2", "2");
                StartCoroutine(CheckLoginStatus());
            }

            if (Input.GetKeyDown(KeyCode.R))     //로그인 테스트용 ㅡㅡ
            {
                photonManager.OnClickLogin("hw01208@naver.com", "123");
                StartCoroutine(CheckLoginStatus());
            }

            //if (Input.GetKeyDown(KeyCode.M))     //닉네임 테스트용 ㅡㅡ  (중복)
            //{
            //    photonManager.OnClickGidButton("master");
            //    StartCoroutine(CheckGidStatus());
            //}

            //if (Input.GetKeyDown(KeyCode.Z))     //닉네임 테스트용 ㅡㅡ  (성공)
            //{
            //    photonManager.OnClickGidButton("endwar1");
            //    StartCoroutine(CheckGidStatus());
            //}

        }

		/// <summary>
		/// Hide the validation message on update. Connect this to OnUpdate.
		/// </summary>
		public void HandleUpdate (string text) {
			keyboard.HideValidationMessage ();
		}

		/// <summary>
		/// Validate the email and simulate a form submission. Connect this to OnSubmit.
		/// </summary>
		public void HandleSubmit (string text) {

            switch (status) {
                case Status.email:
                    keyboard.DisableInput();

                    //if (!ValidateEmail(text))
                    //{
                    //    keyboard.ShowValidationMessage("유효하지 않은 이메일 주소");
                    //    keyboard.EnableInput();
                    //    return;
                    //}

                    StartCoroutine(SubmitEmail(text));
                    break;

                case Status.password:
                    keyboard.DisableInput();
                    
                    keyboard.ShowInfoMessage("처리중...");

                    this.password = keyboard.text;

                    photonManager.OnClickLogin(email, password);

                    StartCoroutine(CheckLoginStatus());
                    break;

                case Status.nickname:
                    keyboard.DisableInput();

                    keyboard.ShowInfoMessage("처리중...");

                    photonManager.OnClickGidButton(keyboard.text);

                    StartCoroutine(CheckGidStatus());
                    break;
            }
		}

		public void HandleCancel () {
			Debug.Log ("Cancelled keyboard input!");
		}

		/// <summary>
		/// Pretend to submit the email before resetting.
		/// </summary>
		private IEnumerator SubmitEmail (string email) {
            this.email = email;
			keyboard.ShowSuccessMessage ("이메일 입력 완료");

			yield return new WaitForSeconds (1f);

			keyboard.HideSuccessMessage ();
			keyboard.SetText ("");
			keyboard.EnableInput ();

            status = Status.password;
            inputLabel.text = "비밀번호";
        }

		private bool ValidateEmail (string text) {
			if (!emailValidator.IsMatch (text)) {
				return false;
			}
			return true;
		}

        IEnumerator CheckLoginStatus()
        {
            yield return new WaitForSeconds(1f);

            if(photonManager.status == PhotonTest.Status.SuccessLogin)
            {
                keyboard.ShowSuccessMessage("로그인 성공, 접속 중...");
                photonManager.WebLogin();
            }

            if (photonManager.status == PhotonTest.Status.InvaildEmail)
            {
                keyboard.ShowValidationMessage("로그인에 실패했습니다.");
                yield return new WaitForSeconds(0.5f);
                keyboard.HideValidationMessage();

                status = Status.email;

                inputLabel.text = "이메일";
            }

            if(photonManager.status == PhotonTest.Status.WaitGid)
            {
                keyboard.ShowSuccessMessage("로그인 성공, 게임 아이디를 설정 해 주세요!");
                yield return new WaitForSeconds(1f);
                status = Status.nickname;
                inputLabel.text = "닉네임 설정";

                keyboard.HideInfoMessage();
            }

            keyboard.SetText("");
            keyboard.EnableInput();

            //if (photonManager.status == PhotonTest.Status.InvaildPassword)
            //{
            //    keyboard.ShowValidationMessage("잘못된 비밀번호 입니다");
            //}
        }

        IEnumerator CheckGidStatus()
        {
            yield return new WaitForSeconds(1f);

            keyboard.HideInfoMessage();

            switch (photonManager.status)
            {
                case PhotonTest.Status.ExistGid:
                    keyboard.ShowValidationMessage("이미 존재하는 게임 아이디입니다.");
                    yield return new WaitForSeconds(1f);
                    keyboard.HideValidationMessage();
                    keyboard.SetText("");
                    keyboard.EnableInput();
                    break;

                case PhotonTest.Status.NoSpaceOrSpecialGid:
                    keyboard.ShowValidationMessage("공백이나 특수 문자 사용은 불가능합니다.");
                    yield return new WaitForSeconds(1f);
                    keyboard.HideValidationMessage();
                    keyboard.SetText("");
                    keyboard.EnableInput();
                    break;

                case PhotonTest.Status.InvaildGid:
                    keyboard.ShowValidationMessage("게임 아이디가 너무 짧거나 깁니다.");
                    yield return new WaitForSeconds(1f);
                    keyboard.HideValidationMessage();
                    keyboard.SetText("");
                    keyboard.EnableInput();
                    break;

                case PhotonTest.Status.SuccessGid:
                    keyboard.ShowSuccessMessage("생성 완료, 접속 중...");
                    photonManager.WebLogin();
                    break;

                case PhotonTest.Status.WaitGid:
                    StartCoroutine(CheckGidStatus());
                    break;
            }
        }
	}
}