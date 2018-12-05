﻿using Desktop;
using SPNATI_Character_Editor.Activities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Controls
{
	public partial class ValidationControl : UserControl
	{
		private Dictionary<Character, List<ValidationError>> _warnings = new Dictionary<Character, List<ValidationError>>();

		private Character _character;
		private CancellationTokenSource _cancelToken;
		public bool IsBusy { get; private set; }

		public ValidationControl()
		{
			InitializeComponent();
			PopulateFilters();
		}

		/// <summary>
		/// Performs validation for a single character, or all characters
		/// </summary>
		/// <param name="character">Character to validate. Pass null to validate all.</param>
		public void DoValidation(Character character)
		{
			if (character != null)
			{
				Validate(character);
			}
			else
			{
				ValidateAll();
			}
		}

		private void Validate(Character character)
		{
			pnlValid.Visible = false;
			List<ValidationError> warnings;
			bool valid = CharacterValidator.Validate(character, Listing.Instance, out warnings);
			if (valid)
			{
				pnlValid.Visible = true;
				pnlValid.BringToFront();
			}
			else
			{
				_warnings[character] = warnings;
				PopulateWarnings();
			}
		}

		private void PopulateWarnings()
		{
			cmdGoTo.Enabled = false;
			pnlWarnings.BringToFront();
			lstCharacters.Items.Clear();
			if (_warnings == null)
			{
				return;
			}
			foreach (Character c in _warnings.Keys)
			{
				lstCharacters.Items.Add(c);
			}
			lstCharacters.SelectedIndex = 0;
		}

		private async void ValidateAll()
		{
			IsBusy = true;

			pnlProgress.Visible = true;
			pnlProgress.BringToFront();
			progressBar.Value = 0;
			progressBar.Maximum = CharacterDatabase.Count;
			Enabled = false;

			int count = CharacterDatabase.Count;
			var progressUpdate = new Progress<Character>(next => {
				if (progressBar.Value < progressBar.Maximum)
				{
					progressBar.Value++;
				}
				lblProgress.Text = $"Validating {next}...";
			});

			_cancelToken = new CancellationTokenSource();
			CancellationToken token = _cancelToken.Token;

			try
			{
				_warnings = await ValidateAll(progressUpdate, token);
				PopulateWarnings();
			}
			finally
			{
				pnlProgress.Visible = false;
				Enabled = true;
				IsBusy = false;
			}
		}

		private Task<Dictionary<Character, List<ValidationError>>> ValidateAll(IProgress<Character> progress, CancellationToken cancelToken)
		{
			return Task.Run(() =>
			{
				try
				{
					Dictionary<Character, List<ValidationError>> allWarnings = new Dictionary<Character, List<ValidationError>>();
					foreach (Character c in CharacterDatabase.Characters)
					{
						OpponentStatus status = Listing.Instance.GetCharacterStatus(c.FolderName);
						if (status == OpponentStatus.Incomplete || status == OpponentStatus.Offline)
							continue; //don't validate characters that aren't in the main opponents folder, since they're likely to have errors but aren't being actively worked on
						progress.Report(c);
						List<ValidationError> warnings;
						if (!CharacterValidator.Validate(c, Listing.Instance, out warnings))
						{
							allWarnings[c] = warnings;
						}
						cancelToken.ThrowIfCancellationRequested();
					}
					return allWarnings;
				}
				catch (OperationCanceledException)
				{
					return null;
				}
			}, cancelToken);
		}

		/// <summary>
		/// Cancels anything currently underway
		/// </summary>
		public void Cancel()
		{
			_cancelToken.Cancel();
		}

		/// <summary>
		/// Sets up the filters listbox
		/// </summary>
		private void PopulateFilters()
		{
			foreach (ValidationFilterLevel level in Enum.GetValues(typeof(ValidationFilterLevel)))
			{
				if (level == ValidationFilterLevel.None)
					continue;
				lstFilters.Items.Add(level);
				if (level != ValidationFilterLevel.Minor)
					lstFilters.SelectedItems.Add(level);
			}
		}

		/// <summary>
		/// Creates a validation filter level from the filters list box
		/// </summary>
		/// <returns></returns>
		private ValidationFilterLevel GetFilterLevel()
		{
			ValidationFilterLevel level = ValidationFilterLevel.None;
			foreach (object item in lstFilters.SelectedItems)
			{
				if (item is ValidationFilterLevel)
				{
					level |= (ValidationFilterLevel)item;
				}
			}

			return level;
		}

		private void lstCharacters_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PopulateWarnings(lstCharacters.SelectedItem as Character);
		}

		private void PopulateWarnings(Character c)
		{
			_character = c;
			lstWarnings.Items.Clear();
			if (c == null)
			{
				return;
			}
			else
			{
				c.Behavior.BuildStageTree(c);
				ValidationFilterLevel filterLevel = GetFilterLevel();
				List<ValidationError> warnings;
				if (_warnings.TryGetValue(c, out warnings))
				{
					foreach (ValidationError error in warnings)
					{
						if (CharacterValidator.IsInFilter(filterLevel, error.Level))
						{
							lstWarnings.Items.Add(error);
						}
					}
				}
			}
		}

		private void lstFilters_SelectedIndexChanged(object sender, EventArgs e)
		{
			PopulateWarnings(lstCharacters.SelectedItem as Character);
		}

		private void lstWarnings_SelectedIndexChanged(object sender, EventArgs e)
		{
			ValidationError error = lstWarnings.SelectedItem as ValidationError;
			cmdGoTo.Enabled = (error != null && error.Context != null);
		}

		private void lstWarnings_DoubleClick(object sender, EventArgs e)
		{
			ValidationError error = lstWarnings.SelectedItem as ValidationError;
			GotoError(error);
		}

		private void cmdGoTo_Click(object sender, EventArgs e)
		{
			ValidationError error = lstWarnings.SelectedItem as ValidationError;
			GotoError(error);
		}

		private void GotoError(ValidationError error)
		{
			if (error == null || error.Context == null) { return; }

			Shell.Instance.Launch<Character, DialogueEditor>(_character, error.Context);
		}
	}
}
