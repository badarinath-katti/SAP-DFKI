package sap.corp.emea.utility;

public class TupleWrapper {

	public static class Tuple<T1, T2> {

		private final T1 item1;
		private final T2 item2;

		public Tuple(T1 item1, T2 item2) {
			this.item1 = item1;
			this.item2 = item2;
		}

		public final T1 getItem1() {
			return item1;
		}

		public final T2 getItem2() {
			return item2;
		}
	}
}
