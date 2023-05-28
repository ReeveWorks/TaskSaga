using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaskSaga.Services
{
    public class RestrictionService
    {
        public RestrictionService()
        {
        }

        public void CheckEmptyEntry(Entry entry, Label noticeLabel, string noticeText)
        {
            bool isEmpty = string.IsNullOrWhiteSpace(entry.Text);
            LabelVisibility(noticeLabel, isEmpty, noticeText);
        }

        public void CheckEntryLength(Entry entry, Label noticeLabel, string noticeText, int minLength)
        {
            bool isBelowMinLength = entry.Text.Length < minLength;
            LabelVisibility(noticeLabel, isBelowMinLength, noticeText);
        }

        public void CheckEntryMatch(Entry entry1, Entry entry2, Label noticeLabel, string noticeText)
        {
            bool isMatch = entry1.Text != entry2.Text;
            LabelVisibility(noticeLabel, isMatch, noticeText);
        }

        public bool CheckLabelVisibility(Label[] labelList)
        {
            foreach (Label label in labelList)
            {
                if (label.IsVisible)
                {
                    return true;
                }
            }

            return false;
        }

        public void LabelVisibility(Label noticeLabel, bool isVisibile, string noticeText)
        {
            noticeLabel.IsVisible = isVisibile;
            noticeLabel.Text = isVisibile ? noticeText : string.Empty;
        }
    }
}
