// Copyright 2003 Eric Marchesin - <eric.marchesin@laposte.net>
//
// This source file(s) may be redistributed by any means PROVIDING they
// are not sold for profit without the authors expressed written consent,
// and providing that this notice and the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------
using System;
using System.Collections;


namespace EMK.Collections
{
	/// <summary>
	/// The SortableList allows to maintain a list sorted as long as needed.
	/// If no IComparer interface has been provided at construction, then the list expects the Objects to implement IComparer.
	/// If the list is not sorted it behaves like an ordinary list.
	/// When sorted, the list's "Add" method will put new objects at the right place.
	/// As well the "Contains" and "IndexOf" methods will perform a binary search.
	/// </summary>
	[Serializable]
	public class SortableList : IList, ICloneable
	{
		private ArrayList _List;
		private IComparer _Comparer;
		private bool _UseObjectsComparison;
		private bool _IsSorted;
		private bool _KeepSorted;
		private bool _AddDuplicates;

		/// <summary>
		/// Default constructor.
		/// Since no IComparer is provided here, added objects must implement the IComparer interface.
		/// </summary>
		public SortableList() { InitProperties(null, 0); }

		/// <summary>
		/// Constructor.
		/// Since no IComparer is provided, added objects must implement the IComparer interface.
		/// </summary>
		/// <param name="Capacity">Capacity of the list (<see cref="ArrayList.Capacity">ArrayList.Capacity</see>)</param>
		public SortableList(int Capacity) { InitProperties(null, Capacity); }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Comparer">Will be used to compare added elements for sort and search operations.</param>
		public SortableList(IComparer Comparer) { InitProperties(Comparer, 0); }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Comparer">Will be used to compare added elements for sort and search operations.</param>
		/// <param name="Capacity">Capacity of the list (<see cref="ArrayList.Capacity">ArrayList.Capacity</see>)</param>
		public SortableList(IComparer Comparer, int Capacity) { InitProperties(Comparer, Capacity); }

		/// <summary>
		/// 'Get only' property that indicates if the list is sorted.
		/// </summary>
		public bool IsSorted { get { return _IsSorted; } }

		/// <summary>
		/// Get : Indicates if the list must be kept sorted from now on.
		/// Set : Tells the list if it must stay sorted or not. Impossible to set to true if the list is not sorted.
		/// <see cref="KeepSorted">KeepSorted</see>==true implies that <see cref="IsSorted">IsSorted</see>==true
		/// </summary>
		/// <exception cref="InvalidOperationException">Cannot be set to true if the list is not sorted yet.</exception>
		public bool KeepSorted
		{
			set
			{
				if ( value==true && !_IsSorted ) throw new InvalidOperationException("The SortableList can only be kept sorted if it is sorted.");
				_KeepSorted = value;
			}
			get { return _KeepSorted; }
		}

		/// <summary>
		/// If set to true, it will not be possible to add an object to the list if its value is already in the list.
		/// </summary>
		public bool AddDuplicates { set { _AddDuplicates = value; } get { return _AddDuplicates; } }

		/// <summary>
		/// IList implementation.
		/// Gets - or sets - object's value at a specified index.
		/// The set operation is impossible if the <see cref="KeepSorted">KeepSorted</see> property is set to true.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">Index is less than zero or Index is greater than Count.</exception>
		/// <exception cref="InvalidOperationException">[] operator cannot be used to set a value if KeepSorted property is set to true.</exception>
		public object this[int Index]
		{
			get
			{
				if ( Index>=_List.Count || Index<0 ) throw new ArgumentOutOfRangeException("Index is less than zero or Index is greater than Count.");
				return _List[Index];
			}
			set
			{
				if ( _KeepSorted ) throw new InvalidOperationException("[] operator cannot be used to set a value if KeepSorted property is set to true.");
				if ( Index>=_List.Count || Index<0 ) throw new ArgumentOutOfRangeException("Index is less than zero or Index is greater than Count.");
				if ( ObjectIsCompliant(value) )
				{
					object OBefore = Index>0 ? _List[Index-1] : null;
					object OAfter = Index<Count-1 ? _List[Index+1] : null;
					if ( OBefore!=null && _Comparer.Compare(OBefore, value)>0 || OAfter!=null && _Comparer.Compare(value, OAfter)>0 ) _IsSorted = false;
					_List[Index] = value;
				}
			}
		}

		/// <summary>
		/// IList implementation.
		/// If the <see cref="KeepSorted">KeepSorted</see> property is set to true, the object will be added at the right place.
		/// Else it will be added at the end of the list.
		/// </summary>
		/// <param name="O">The object to add.</param>
		/// <returns>The index where the object has been added.</returns>
		/// <exception cref="ArgumentException">The SortableList is set to use object's IComparable interface, and the specifed object does not implement this interface.</exception>
		public int Add(object O)
		{
			int Return = -1;
			if ( ObjectIsCompliant(O) )
			{
				if ( _KeepSorted )
				{
					int Index = IndexOf(O);
					int NewIndex = Index>=0 ? Index : -Index-1;
					if (NewIndex>=Count) _List.Add(O);
					else _List.Insert(NewIndex, O);
					Return = NewIndex;
				}
				else
				{
					_IsSorted = false;
					Return = _List.Add(O);
				}
			}
			return Return;
		}

		/// <summary>
		/// IList implementation.
		/// Search for a specified object in the list.
		/// If the list is sorted, a <see cref="ArrayList.BinarySearch">BinarySearch</see> is performed using IComparer interface.
		/// Else the <see cref="Equals">Object.Equals</see> implementation is used.
		/// </summary>
		/// <param name="O">The object to look for</param>
		/// <returns>true if the object is in the list, otherwise false.</returns>
		public bool Contains(object O)
		{
			return _IsSorted ? _List.BinarySearch(O, _Comparer)>=0 : _List.Contains(O);
		}

		/// <summary>
		/// IList implementation.
		/// Returns the index of the specified object in the list.
		/// If the list is sorted, a <see cref="ArrayList.BinarySearch">BinarySearch</see> is performed using IComparer interface.
		/// Else the <see cref="Equals">Object.Equals</see> implementation of objects is used.
		/// </summary>
		/// <param name="O">The object to locate.</param>
		/// <returns>
		/// If the object has been found, a positive integer corresponding to its position.
		/// If the objects has not been found, a negative integer which is the bitwise complement of the index of the next element.
		/// </returns>
		public int IndexOf(object O)
		{
			int Result = -1;
			if ( _IsSorted )
			{
				Result = _List.BinarySearch(O, _Comparer);
				while ( Result>0 && _List[Result-1].Equals(O) ) Result--; // We want to point at the FIRST occurence
			}
			else Result = _List.IndexOf(O);
			return Result;
		}

		/// <summary>
		/// IList implementation.
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		public bool IsFixedSize { get { return _List.IsFixedSize ; } }

		/// <summary>
		/// IList implementation.
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		public bool IsReadOnly { get { return _List.IsReadOnly; } }

		/// <summary>
		/// IList implementation.
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		public void Clear() { _List.Clear(); }

		/// <summary>
		/// IList implementation.
		/// Inserts an objects at a specified index.
		/// Cannot be used if the list has its KeepSorted property set to true.
		/// </summary>
		/// <param name="Index">The index before which the object must be added.</param>
		/// <param name="O">The object to add.</param>
		/// <exception cref="ArgumentException">The SortableList is set to use object's IComparable interface, and the specifed object does not implement this interface.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Index is less than zero or Index is greater than Count.</exception>
		/// <exception cref="InvalidOperationException">If the object is added at the specify index, the list will not be sorted any more and the <see cref="KeepSorted"/> property is set to true.</exception>
		public void Insert(int Index, object O)
		{
			if ( _KeepSorted ) throw new InvalidOperationException("Insert method cannot be called if KeepSorted property is set to true.");
			if ( Index>=_List.Count || Index<0 ) throw new ArgumentOutOfRangeException("Index is less than zero or Index is greater than Count.");
			if ( ObjectIsCompliant(O) )
			{
				object OBefore = Index>0 ? _List[Index-1] : null;
				object OAfter = _List[Index];
				if ( OBefore!=null && _Comparer.Compare(OBefore, O)>0 || OAfter!=null && _Comparer.Compare(O, OAfter)>0 ) _IsSorted = false;
				_List.Insert(Index, O);
			}
		}

		/// <summary>
		/// IList implementation.
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		/// <param name="Value">The object whose value must be removed if found in the list.</param>
		public void Remove(object Value) { _List.Remove(Value); }

		/// <summary>
		/// IList implementation.
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		/// <param name="Index">Index of object to remove.</param>
		public void RemoveAt(int Index) { _List.RemoveAt(Index); }

		/// <summary>
		/// IList.ICollection implementation.
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(Array array, int arrayIndex) { _List.CopyTo(array, arrayIndex); }
		
		/// <summary>
		/// IList.ICollection implementation.
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		public int Count { get { return _List.Count; } }

		/// <summary>
		/// IList.ICollection implementation.
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		public bool IsSynchronized { get { return _List.IsSynchronized; } }

		/// <summary>
		/// IList.ICollection implementation.
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		public object SyncRoot { get { return _List.SyncRoot; } }

		/// <summary>
		/// IList.IEnumerable implementation.
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		/// <returns>Enumerator on the list.</returns>
		public IEnumerator GetEnumerator() { return _List.GetEnumerator(); }

		/// <summary>
		/// ICloneable implementation.
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		/// <returns>Cloned object.</returns>
		public object Clone()
		{
			SortableList Clone = new SortableList(_Comparer, _List.Capacity);
			Clone._List = (ArrayList)_List.Clone();
			Clone._AddDuplicates = _AddDuplicates;
			Clone._IsSorted = _IsSorted;
			Clone._KeepSorted = _KeepSorted;
			return Clone;
		}

		/// <summary>
		/// Idem IndexOf(object), but starting at a specified position in the list
		/// </summary>
		/// <param name="O">The object to locate.</param>
		/// <param name="Start">The index for start position.</param>
		/// <returns></returns>
		public int IndexOf(object O, int Start)
		{
			int Result = -1;
			if ( _IsSorted )
			{
				Result = _List.BinarySearch(Start, _List.Count-Start, O, _Comparer);
				while ( Result>Start && _List[Result-1].Equals(O) ) Result--; // We want to point at the first occurence
			}
			else Result = _List.IndexOf(O, Start);
			return Result;
		}

		/// <summary>
		/// Defines an equality for two objects
		/// </summary>
		public delegate bool Equality(object O1, object O2);

		/// <summary>
		/// Idem IndexOf(object), but with a specified equality function
		/// </summary>
		/// <param name="O">The object to locate.</param>
		/// <param name="AreEqual">Equality function to use for the search.</param>
		/// <returns></returns>
		public int IndexOf(object O, Equality AreEqual)
		{
			for (int i=0; i<_List.Count; i++)
				if ( AreEqual(_List[i], O) ) return i;
			return -1;
		}

		/// <summary>
		/// Idem IndexOf(object), but with a start index and a specified equality function
		/// </summary>
		/// <param name="O">The object to locate.</param>
		/// <param name="Start">The index for start position.</param>
		/// <param name="AreEqual">Equality function to use for the search.</param>
		/// <returns></returns>
		public int IndexOf(object O, int Start, Equality AreEqual)
		{
			if ( Start<0 || Start>=_List.Count ) throw new ArgumentException("Start index must belong to [0; Count-1].");
			for (int i=Start; i<_List.Count; i++)
				if ( AreEqual(_List[i], O) ) return i;
			return -1;
		}

		/// <summary>
		/// Idem <see cref="ArrayList">ArrayList</see>
		/// </summary>
		public int Capacity { get {return _List.Capacity; } set { _List.Capacity = value; } }

		/// <summary>
		/// Object.ToString() override.
		/// Build a string to represent the list.
		/// </summary>
		/// <returns>The string refecting the list.</returns>
		public override string ToString()
		{
			string OutString = "{";
			for (int i=0; i<_List.Count; i++)
				OutString += _List[i].ToString() + (i!=_List.Count-1 ? "; " : "}");
			return OutString;
		}

		/// <summary>
		/// Object.Equals() override.
		/// </summary>
		/// <returns>true if object is equal to this, otherwise false.</returns>
		public override bool Equals(object O)
		{
			SortableList SL = (SortableList)O;
			if ( SL.Count!=Count ) return false;
			for (int i=0; i<Count; i++)
				if ( !SL[i].Equals(this[i]) ) return false;
			return true;
		}

		/// <summary>
		/// Object.GetHashCode() override.
		/// </summary>
		/// <returns>HashCode value.</returns>
		public override int GetHashCode() { return _List.GetHashCode(); }

		/// <summary>
		/// Sorts the elements in the list using <see cref="ArrayList.Sort">ArrayList.Sort</see>.
		/// Does nothing if the list is already sorted.
		/// </summary>
		public void Sort()
		{
			if (_IsSorted) return;
			_List.Sort(_Comparer);
			_IsSorted = true;
		}

		/// <summary>
		/// If the <see cref="KeepSorted">KeepSorted</see> property is set to true, the object will be added at the right place.
		/// Else it will be appended to the list.
		/// </summary>
		/// <param name="C">The object to add.</param>
		/// <returns>The index where the object has been added.</returns>
		/// <exception cref="ArgumentException">The SortableList is set to use object's IComparable interface, and the specifed object does not implement this interface.</exception>
		public void AddRange(ICollection C)
		{
			if ( _KeepSorted ) foreach (object O in C) Add(O);
			else _List.AddRange(C);
		}

		/// <summary>
		/// Inserts a collection of objects at a specified index.
		/// Should not be used if the list is the KeepSorted property is set to true.
		/// </summary>
		/// <param name="Index">The index before which the objects must be added.</param>
		/// <param name="C">The object to add.</param>
		/// <exception cref="ArgumentException">The SortableList is set to use objects's IComparable interface, and the specifed object does not implement this interface.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Index is less than zero or Index is greater than Count.</exception>
		/// <exception cref="InvalidOperationException">If the object is added at the specify index, the list will not be sorted any more and the <see cref="KeepSorted"/> property is set to true.</exception>
		public void InsertRange(int Index, ICollection C)
		{
			if ( _KeepSorted ) foreach (object O in C) Insert(Index++, O);
			else _List.InsertRange(Index, C);
		}

		/// <summary>
		/// Limits the number of occurrences of a specified value.
		/// Same values are equals according to the Equals() method of objects in the list.
		/// The first occurrences encountered are kept.
		/// </summary>
		/// <param name="Value">Value whose occurrences number must be limited.</param>
		/// <param name="NbValuesToKeep">Number of occurrences to keep</param>
		public void LimitNbOccurrences(object Value, int NbValuesToKeep)
		{
			if (Value==null) throw new ArgumentNullException("Value");
			int Pos = 0;
			while ( (Pos=IndexOf(Value, Pos)) >= 0 )
			{
				 if ( NbValuesToKeep<=0 ) _List.RemoveAt(Pos);
				else { Pos++; NbValuesToKeep--; }
				if ( _IsSorted && _Comparer.Compare(_List[Pos], Value)>0 ) break; // No need to follow
			}
		}

		/// <summary>
		/// Removes all duplicates in the list.
		/// Each value encountered will have only one representant.
		/// </summary>
		public void RemoveDuplicates()
		{
			int PosIt;
			if (_IsSorted)
			{
				PosIt = 0;
				while ( PosIt<Count-1 )
				{
					if ( _Comparer.Compare(this[PosIt], this[PosIt+1])==0 ) RemoveAt(PosIt);
					else PosIt++;
				}
			}
			else
			{
				int Left = 0;
				while ( Left>=0 )
				{
					PosIt = Left+1;
					while (PosIt>0)
					{
						if ( Left!=PosIt && _Comparer.Compare(this[Left], this[PosIt])==0 ) RemoveAt(PosIt);
						else PosIt++;
					}
					Left++;
				}
			}
		}

		/// <summary>
		/// Returns the object of the list whose value is minimum
		/// </summary>
		/// <returns>The minimum object in the list</returns>
		public int IndexOfMin()
		{
			int RetInt = -1;
			if ( _List.Count>0 )
			{
				RetInt = 0;
				object RetObj = _List[0];
				if ( !_IsSorted )
				{
					for ( int i=1; i<_List.Count; i++ )
						if ( _Comparer.Compare(RetObj, _List[i])>0 )
						{
							RetObj = _List[i];
							RetInt = i;
						}
				}
			}
			return RetInt;
		}

		/// <summary>
		/// Returns the object of the list whose value is maximum
		/// </summary>
		/// <returns>The maximum object in the list</returns>
		public int IndexOfMax()
		{
			int RetInt = -1;
			if ( _List.Count>0 )
			{
				RetInt = _List.Count-1;
				object RetObj = _List[_List.Count-1];
				if ( !_IsSorted )
				{
					for ( int i=_List.Count-2; i>=0; i-- )
						if ( _Comparer.Compare(RetObj, _List[i])<0 )
						{
							RetObj = _List[i];
							RetInt = i;
						}
				}
			}
			return RetInt;
		}

		private bool ObjectIsCompliant(object O)
		{
			if ( _UseObjectsComparison && !(O is IComparable) ) throw new ArgumentException("The SortableList is set to use the IComparable interface of objects, and the object to add does not implement the IComparable interface.");
			if ( !_AddDuplicates && Contains(O) ) return false;
			return true;
		}

		private class Comparison : IComparer
		{
			public int Compare(object O1, object O2)
			{
				IComparable C = O1 as IComparable;
				return C.CompareTo(O2);
			}
		}

		private void InitProperties(IComparer Comparer, int Capacity)
		{
			if ( Comparer!=null )
			{
				_Comparer = Comparer;
				_UseObjectsComparison = false;
			}
			else
			{
				_Comparer = new Comparison();
				_UseObjectsComparison = true;
			}
			_List = Capacity>0 ? new ArrayList(Capacity) : new ArrayList();
			_IsSorted = true;
			_KeepSorted = true;
			_AddDuplicates = true;
		}
	}
}
