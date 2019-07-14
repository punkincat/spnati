﻿using System;
using System.Windows.Forms;

namespace Desktop.CommonControls
{
	public class RecordColumn : DataGridViewColumn
	{
		private RecordCell _cellTemplate;

		public RecordColumn()
		{
			CellTemplate = _cellTemplate = new RecordCell();
		}

		public Type RecordType
		{
			get { return _cellTemplate.RecordType; }
			set
			{
				_cellTemplate.RecordType = value;
				if (DataGridView != null)
				{
					DataGridViewRowCollection rows = DataGridView.Rows;
					for (int i = 0; i < rows.Count; i++)
					{
						DataGridViewRow row = rows.SharedRow(i);
						RecordCell cell = row.Cells[Index] as RecordCell;
						if (cell != null)
						{
							cell.RecordType = value;
						}
					}
					DataGridView.InvalidateColumn(Index);
				}
			}
		}

		public override object Clone()
		{
			RecordColumn copy = base.Clone() as RecordColumn;
			copy.RecordType = RecordType;
			return copy;
		}
	}
}
