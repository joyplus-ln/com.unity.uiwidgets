
//part of dart.async;


using System;
using Unity.UIWidgets.core;

namespace Unity.UIWidgets.async {
    

public static class  _stream{
 public delegate  void _TimerCallback();
}


// abstract class Stream<T> {
//   Stream();

 
//   const Stream._internal();


//   const factory Stream.empty() = _EmptyStream<T>;

  
//   // @Since("2.5")
//   factory Stream.value(T value) =>
//       (_AsyncStreamController<T>(null, null, null, null)
//             .._add(value)
//             .._closeUnchecked())
//           .stream;

//   // @Since("2.5")
//   factory Stream.error(Object error, [StackTrace stackTrace]) {
//     ArgumentError.checkNotNull(error, "error");
//     return (_AsyncStreamController<T>(null, null, null, null)
//           .._addError(error, stackTrace ?? AsyncError.defaultStackTrace(error))
//           .._closeUnchecked())
//         .stream;
//   }

//   factory Stream.fromFuture(Future<T> future) {
//     // Use the controller's buffering to fill in the value even before
//     // the stream has a listener. For a single value, it's not worth it
//     // to wait for a listener before doing the `then` on the future.
//     _StreamController<T> controller =
//         new _SyncStreamController<T>(null, null, null, null);
//     future.then((value) {
//       controller._add(value);
//       controller._closeUnchecked();
//     }, onError: (error, stackTrace) {
//       controller._addError(error, stackTrace);
//       controller._closeUnchecked();
//     });
//     return controller.stream;
//   }

//   factory Stream.fromFutures(Iterable<Future<T>> futures) {
//     _StreamController<T> controller =
//         new _SyncStreamController<T>(null, null, null, null);
//     int count = 0;
//     // Declare these as variables holding closures instead of as
//     // function declarations.
//     // This avoids creating a new closure from the functions for each future.
//     var onValue = (T value) {
//       if (!controller.isClosed) {
//         controller._add(value);
//         if (--count == 0) controller._closeUnchecked();
//       }
//     };
//     var onError = (error, StackTrace stack) {
//       if (!controller.isClosed) {
//         controller._addError(error, stack);
//         if (--count == 0) controller._closeUnchecked();
//       }
//     };
//     // The futures are already running, so start listening to them immediately
//     // (instead of waiting for the stream to be listened on).
//     // If we wait, we might not catch errors in the futures in time.
//     for (var future in futures) {
//       count++;
//       future.then(onValue, onError: onError);
//     }
//     // Use schedule microtask since controller is sync.
//     if (count == 0) scheduleMicrotask(controller.close);
//     return controller.stream;
//   }

//   factory Stream.fromIterable(Iterable<T> elements) {
//     return new _GeneratedStreamImpl<T>(
//         () => new _IterablePendingEvents<T>(elements));
//   }

//   factory Stream.periodic(Duration period,
//       [T computation(int computationCount)]) {
//     Timer timer;
//     int computationCount = 0;
//     StreamController<T> controller;
//     // Counts the time that the Stream was running (and not paused).
//     Stopwatch watch = new Stopwatch();

//     void sendEvent() {
//       watch.reset();
//       T data;
//       if (computation != null) {
//         try {
//           data = computation(computationCount++);
//         } catch (e, s) {
//           controller.addError(e, s);
//           return;
//         }
//       }
//       controller.add(data);
//     }

//     void startPeriodicTimer() {
//       assert(timer == null);
//       timer = new Timer.periodic(period, (Timer timer) {
//         sendEvent();
//       });
//     }

//     controller = new StreamController<T>(
//         sync: true,
//         onListen: () {
//           watch.start();
//           startPeriodicTimer();
//         },
//         onPause: () {
//           timer.cancel();
//           timer = null;
//           watch.stop();
//         },
//         onResume: () {
//           assert(timer == null);
//           Duration elapsed = watch.elapsed;
//           watch.start();
//           timer = new Timer(period - elapsed, () {
//             timer = null;
//             startPeriodicTimer();
//             sendEvent();
//           });
//         },
//         onCancel: () {
//           if (timer != null) timer.cancel();
//           timer = null;
//           return Future._nullFuture;
//         });
//     return controller.stream;
//   }

//   factory Stream.eventTransformed(
//       Stream source, EventSink mapSink(EventSink<T> sink)) {
//     return new _BoundSinkStream(source, mapSink);
//   }

//   static Stream<T> castFrom<S, T>(Stream<S> source) =>
//       new CastStream<S, T>(source);

//   bool get isBroadcast => false;

//   Stream<T> asBroadcastStream(
//       {void onListen(StreamSubscription<T> subscription),
//       void onCancel(StreamSubscription<T> subscription)}) {
//     return new _AsBroadcastStream<T>(this, onListen, onCancel);
//   }

//   StreamSubscription<T> listen(void onData(T event),
//       {Function onError, void onDone(), bool cancelOnError});

//   Stream<T> where(bool test(T event)) {
//     return new _WhereStream<T>(this, test);
//   }

//   Stream<S> map<S>(S convert(T event)) {
//     return new _MapStream<T, S>(this, convert);
//   }

//   Stream<E> asyncMap<E>(FutureOr<E> convert(T event)) {
//     _StreamControllerBase<E> controller;
//     StreamSubscription<T> subscription;

//     void onListen() {
//       final add = controller.add;
//       assert(controller is _StreamController<E> ||
//           controller is _BroadcastStreamController);
//       final addError = controller._addError;
//       subscription = this.listen((T event) {
//         FutureOr<E> newValue;
//         try {
//           newValue = convert(event);
//         } catch (e, s) {
//           controller.addError(e, s);
//           return;
//         }
//         if (newValue is Future<E>) {
//           subscription.pause();
//           newValue
//               .then(add, onError: addError)
//               .whenComplete(subscription.resume);
//         } else {
//           controller.add(newValue);
//         }
//       }, onError: addError, onDone: controller.close);
//     }

//     if (this.isBroadcast) {
//       controller = new StreamController<E>.broadcast(
//           onListen: onListen,
//           onCancel: () {
//             subscription.cancel();
//           },
//           sync: true);
//     } else {
//       controller = new StreamController<E>(
//           onListen: onListen,
//           onPause: () {
//             subscription.pause();
//           },
//           onResume: () {
//             subscription.resume();
//           },
//           onCancel: () => subscription.cancel(),
//           sync: true);
//     }
//     return controller.stream;
//   }

//   Stream<E> asyncExpand<E>(Stream<E> convert(T event)) {
//     _StreamControllerBase<E> controller;
//     StreamSubscription<T> subscription;
//     void onListen() {
//       assert(controller is _StreamController ||
//           controller is _BroadcastStreamController);
//       subscription = this.listen((T event) {
//         Stream<E> newStream;
//         try {
//           newStream = convert(event);
//         } catch (e, s) {
//           controller.addError(e, s);
//           return;
//         }
//         if (newStream != null) {
//           subscription.pause();
//           controller.addStream(newStream).whenComplete(subscription.resume);
//         }
//       },
//           onError: controller._addError, // Avoid Zone error replacement.
//           onDone: controller.close);
//     }

//     if (this.isBroadcast) {
//       controller = new StreamController<E>.broadcast(
//           onListen: onListen,
//           onCancel: () {
//             subscription.cancel();
//           },
//           sync: true);
//     } else {
//       controller = new StreamController<E>(
//           onListen: onListen,
//           onPause: () {
//             subscription.pause();
//           },
//           onResume: () {
//             subscription.resume();
//           },
//           onCancel: () => subscription.cancel(),
//           sync: true);
//     }
//     return controller.stream;
//   }

//   Stream<T> handleError(Function onError, {bool test(error)}) {
//     return new _HandleErrorStream<T>(this, onError, test);
//   }

//   Stream<S> expand<S>(Iterable<S> convert(T element)) {
//     return new _ExpandStream<T, S>(this, convert);
//   }

//   Future pipe(StreamConsumer<T> streamConsumer) {
//     return streamConsumer.addStream(this).then((_) => streamConsumer.close());
//   }

//   Stream<S> transform<S>(StreamTransformer<T, S> streamTransformer) {
//     return streamTransformer.bind(this);
//   }

//   Future<T> reduce(T combine(T previous, T element)) {
//     _Future<T> result = new _Future<T>();
//     bool seenFirst = false;
//     T value;
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (T element) {
//           if (seenFirst) {
//             _runUserCode(() => combine(value, element), (T newValue) {
//               value = newValue;
//             }, _cancelAndErrorClosure(subscription, result));
//           } else {
//             value = element;
//             seenFirst = true;
//           }
//         },
//         onError: result._completeError,
//         onDone: () {
//           if (!seenFirst) {
//             try {
//               // Throw and recatch, instead of just doing
//               //  _completeWithErrorCallback, e, theError, StackTrace.current),
//               // to ensure that the stackTrace is set on the error.
//               throw IterableElementError.noElement();
//             } catch (e, s) {
//               _completeWithErrorCallback(result, e, s);
//             }
//           } else {
//             result._complete(value);
//           }
//         },
//         cancelOnError: true);
//     return result;
//   }

//   Future<S> fold<S>(S initialValue, S combine(S previous, T element)) {
//     _Future<S> result = new _Future<S>();
//     S value = initialValue;
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (T element) {
//           _runUserCode(() => combine(value, element), (S newValue) {
//             value = newValue;
//           }, _cancelAndErrorClosure(subscription, result));
//         },
//         onError: result._completeError,
//         onDone: () {
//           result._complete(value);
//         },
//         cancelOnError: true);
//     return result;
//   }

//   Future<String> join([String separator = ""]) {
//     _Future<String> result = new _Future<String>();
//     StringBuffer buffer = new StringBuffer();
//     StreamSubscription subscription;
//     bool first = true;
//     subscription = this.listen(
//         (T element) {
//           if (!first) {
//             buffer.write(separator);
//           }
//           first = false;
//           try {
//             buffer.write(element);
//           } catch (e, s) {
//             _cancelAndErrorWithReplacement(subscription, result, e, s);
//           }
//         },
//         onError: result._completeError,
//         onDone: () {
//           result._complete(buffer.toString());
//         },
//         cancelOnError: true);
//     return result;
//   }

//   Future<bool> contains(Object needle) {
//     _Future<bool> future = new _Future<bool>();
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (T element) {
//           _runUserCode(() => (element == needle), (bool isMatch) {
//             if (isMatch) {
//               _cancelAndValue(subscription, future, true);
//             }
//           }, _cancelAndErrorClosure(subscription, future));
//         },
//         onError: future._completeError,
//         onDone: () {
//           future._complete(false);
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future forEach(void action(T element)) {
//     _Future future = new _Future();
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (T element) {
//           // TODO(floitsch): the type should be 'void' and inferred.
//           _runUserCode<dynamic>(() => action(element), (_) {},
//               _cancelAndErrorClosure(subscription, future));
//         },
//         onError: future._completeError,
//         onDone: () {
//           future._complete(null);
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<bool> every(bool test(T element)) {
//     _Future<bool> future = new _Future<bool>();
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (T element) {
//           _runUserCode(() => test(element), (bool isMatch) {
//             if (!isMatch) {
//               _cancelAndValue(subscription, future, false);
//             }
//           }, _cancelAndErrorClosure(subscription, future));
//         },
//         onError: future._completeError,
//         onDone: () {
//           future._complete(true);
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<bool> any(bool test(T element)) {
//     _Future<bool> future = new _Future<bool>();
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (T element) {
//           _runUserCode(() => test(element), (bool isMatch) {
//             if (isMatch) {
//               _cancelAndValue(subscription, future, true);
//             }
//           }, _cancelAndErrorClosure(subscription, future));
//         },
//         onError: future._completeError,
//         onDone: () {
//           future._complete(false);
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<int> get length {
//     _Future<int> future = new _Future<int>();
//     int count = 0;
//     this.listen(
//         (_) {
//           count++;
//         },
//         onError: future._completeError,
//         onDone: () {
//           future._complete(count);
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<bool> get isEmpty {
//     _Future<bool> future = new _Future<bool>();
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (_) {
//           _cancelAndValue(subscription, future, false);
//         },
//         onError: future._completeError,
//         onDone: () {
//           future._complete(true);
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Stream<R> cast<R>() => Stream.castFrom<T, R>(this);
//   Future<List<T>> toList() {
//     List<T> result = <T>[];
//     _Future<List<T>> future = new _Future<List<T>>();
//     this.listen(
//         (T data) {
//           result.add(data);
//         },
//         onError: future._completeError,
//         onDone: () {
//           future._complete(result);
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<Set<T>> toSet() {
//     Set<T> result = new Set<T>();
//     _Future<Set<T>> future = new _Future<Set<T>>();
//     this.listen(
//         (T data) {
//           result.add(data);
//         },
//         onError: future._completeError,
//         onDone: () {
//           future._complete(result);
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<E> drain<E>([E futureValue]) =>
//       listen(null, cancelOnError: true).asFuture<E>(futureValue);

//   Stream<T> take(int count) {
//     return new _TakeStream<T>(this, count);
//   }

//   Stream<T> takeWhile(bool test(T element)) {
//     return new _TakeWhileStream<T>(this, test);
//   }

//   Stream<T> skip(int count) {
//     return new _SkipStream<T>(this, count);
//   }

//   Stream<T> skipWhile(bool test(T element)) {
//     return new _SkipWhileStream<T>(this, test);
//   }

//   Stream<T> distinct([bool equals(T previous, T next)]) {
//     return new _DistinctStream<T>(this, equals);
//   }

//   Future<T> get first {
//     _Future<T> future = new _Future<T>();
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (T value) {
//           _cancelAndValue(subscription, future, value);
//         },
//         onError: future._completeError,
//         onDone: () {
//           try {
//             throw IterableElementError.noElement();
//           } catch (e, s) {
//             _completeWithErrorCallback(future, e, s);
//           }
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<T> get last {
//     _Future<T> future = new _Future<T>();
//     T result;
//     bool foundResult = false;
//     listen(
//         (T value) {
//           foundResult = true;
//           result = value;
//         },
//         onError: future._completeError,
//         onDone: () {
//           if (foundResult) {
//             future._complete(result);
//             return;
//           }
//           try {
//             throw IterableElementError.noElement();
//           } catch (e, s) {
//             _completeWithErrorCallback(future, e, s);
//           }
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<T> get single {
//     _Future<T> future = new _Future<T>();
//     T result;
//     bool foundResult = false;
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (T value) {
//           if (foundResult) {
//             // This is the second element we get.
//             try {
//               throw IterableElementError.tooMany();
//             } catch (e, s) {
//               _cancelAndErrorWithReplacement(subscription, future, e, s);
//             }
//             return;
//           }
//           foundResult = true;
//           result = value;
//         },
//         onError: future._completeError,
//         onDone: () {
//           if (foundResult) {
//             future._complete(result);
//             return;
//           }
//           try {
//             throw IterableElementError.noElement();
//           } catch (e, s) {
//             _completeWithErrorCallback(future, e, s);
//           }
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<T> firstWhere(bool test(T element), {T orElse()}) {
//     _Future<T> future = new _Future();
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (T value) {
//           _runUserCode(() => test(value), (bool isMatch) {
//             if (isMatch) {
//               _cancelAndValue(subscription, future, value);
//             }
//           }, _cancelAndErrorClosure(subscription, future));
//         },
//         onError: future._completeError,
//         onDone: () {
//           if (orElse != null) {
//             _runUserCode(orElse, future._complete, future._completeError);
//             return;
//           }
//           try {
//             throw IterableElementError.noElement();
//           } catch (e, s) {
//             _completeWithErrorCallback(future, e, s);
//           }
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<T> lastWhere(bool test(T element), {T orElse()}) {
//     _Future<T> future = new _Future();
//     T result;
//     bool foundResult = false;
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (T value) {
//           _runUserCode(() => true == test(value), (bool isMatch) {
//             if (isMatch) {
//               foundResult = true;
//               result = value;
//             }
//           }, _cancelAndErrorClosure(subscription, future));
//         },
//         onError: future._completeError,
//         onDone: () {
//           if (foundResult) {
//             future._complete(result);
//             return;
//           }
//           if (orElse != null) {
//             _runUserCode(orElse, future._complete, future._completeError);
//             return;
//           }
//           try {
//             throw IterableElementError.noElement();
//           } catch (e, s) {
//             _completeWithErrorCallback(future, e, s);
//           }
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<T> singleWhere(bool test(T element), {T orElse()}) {
//     _Future<T> future = new _Future<T>();
//     T result;
//     bool foundResult = false;
//     StreamSubscription subscription;
//     subscription = this.listen(
//         (T value) {
//           _runUserCode(() => true == test(value), (bool isMatch) {
//             if (isMatch) {
//               if (foundResult) {
//                 try {
//                   throw IterableElementError.tooMany();
//                 } catch (e, s) {
//                   _cancelAndErrorWithReplacement(subscription, future, e, s);
//                 }
//                 return;
//               }
//               foundResult = true;
//               result = value;
//             }
//           }, _cancelAndErrorClosure(subscription, future));
//         },
//         onError: future._completeError,
//         onDone: () {
//           if (foundResult) {
//             future._complete(result);
//             return;
//           }
//           try {
//             if (orElse != null) {
//               _runUserCode(orElse, future._complete, future._completeError);
//               return;
//             }
//             throw IterableElementError.noElement();
//           } catch (e, s) {
//             _completeWithErrorCallback(future, e, s);
//           }
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Future<T> elementAt(int index) {
//     ArgumentError.checkNotNull(index, "index");
//     RangeError.checkNotNegative(index, "index");
//     _Future<T> future = new _Future<T>();
//     StreamSubscription subscription;
//     int elementIndex = 0;
//     subscription = this.listen(
//         (T value) {
//           if (index == elementIndex) {
//             _cancelAndValue(subscription, future, value);
//             return;
//           }
//           elementIndex += 1;
//         },
//         onError: future._completeError,
//         onDone: () {
//           future._completeError(
//               new RangeError.index(index, this, "index", null, elementIndex));
//         },
//         cancelOnError: true);
//     return future;
//   }

//   Stream<T> timeout(Duration timeLimit, {void onTimeout(EventSink<T> sink)}) {
//     _StreamControllerBase<T> controller;
//     // The following variables are set on listen.
//     StreamSubscription<T> subscription;
//     Timer timer;
//     Zone zone;
//     _TimerCallback timeout;

//     void onData(T event) {
//       timer.cancel();
//       timer = zone.createTimer(timeLimit, timeout);
//       // It might close the stream and cancel timer, so create recuring Timer
//       // before calling into add();
//       // issue: https://github.com/dart-lang/sdk/issues/37565
//       controller.add(event);
//     }

//     void onError(error, StackTrace stackTrace) {
//       timer.cancel();
//       assert(controller is _StreamController ||
//           controller is _BroadcastStreamController);
//       controller._addError(error, stackTrace); // Avoid Zone error replacement.
//       timer = zone.createTimer(timeLimit, timeout);
//     }

//     void onDone() {
//       timer.cancel();
//       controller.close();
//     }

//     void onListen() {
//       // This is the onListen callback for of controller.
//       // It runs in the same zone that the subscription was created in.
//       // Use that zone for creating timers and running the onTimeout
//       // callback.
//       zone = Zone.current;
//       if (onTimeout == null) {
//         timeout = () {
//           controller.addError(
//               new TimeoutException("No stream event", timeLimit), null);
//         };
//       } else {
//         // TODO(floitsch): the return type should be 'void', and the type
//         // should be inferred.
//         var registeredOnTimeout =
//             zone.registerUnaryCallback<dynamic, EventSink<T>>(onTimeout);
//         var wrapper = new _ControllerEventSinkWrapper<T>(null);
//         timeout = () {
//           wrapper._sink = controller; // Only valid during call.
//           zone.runUnaryGuarded(registeredOnTimeout, wrapper);
//           wrapper._sink = null;
//         };
//       }

//       subscription = this.listen(onData, onError: onError, onDone: onDone);
//       timer = zone.createTimer(timeLimit, timeout);
//     }

//     Future onCancel() {
//       timer.cancel();
//       Future result = subscription.cancel();
//       subscription = null;
//       return result;
//     }

//     controller = isBroadcast
//         ? new _SyncBroadcastStreamController<T>(onListen, onCancel)
//         : new _SyncStreamController<T>(onListen, () {
//             // Don't null the timer, onCancel may call cancel again.
//             timer.cancel();
//             subscription.pause();
//           }, () {
//             subscription.resume();
//             timer = zone.createTimer(timeLimit, timeout);
//           }, onCancel);
//     return controller.stream;
//   }
// }

public abstract class StreamSubscription<T> {
  public abstract Future cancel();

  public abstract void onData(Action<T> handleData);

  public abstract void onError(Action handleError);

  public abstract void onDone(Action handleDone);

  public abstract void pause(Future resumeSignal);

  public abstract void resume();

  public bool  isPaused { get; }

  public abstract Future<E> asFuture<E>(E futureValue);
}

public abstract class EventSink<T> : Sink<T> {
  // public abstract void add(T evt);

  public abstract void addError(object error, string stackTrace);

  // void close();
}

// /** [Stream] wrapper that only exposes the [Stream] interface. */
// class StreamView<T> extends Stream<T> {
//   final Stream<T> _stream;

//   const StreamView(Stream<T> stream)
//       : _stream = stream,
//         super._internal();

//   bool get isBroadcast => _stream.isBroadcast;

//   Stream<T> asBroadcastStream(
//           {void onListen(StreamSubscription<T> subscription),
//           void onCancel(StreamSubscription<T> subscription)}) =>
//       _stream.asBroadcastStream(onListen: onListen, onCancel: onCancel);

//   StreamSubscription<T> listen(void onData(T value),
//       {Function onError, void onDone(), bool cancelOnError}) {
//     return _stream.listen(onData,
//         onError: onError, onDone: onDone, cancelOnError: cancelOnError);
//   }
// }

public interface  StreamConsumer<S> {
   Future addStream(Stream<S> stream);

   // cannot define function with same name
  Future closeConsumer();
}

public abstract class StreamSink<S> : EventSink<S>, StreamConsumer<S> {
  // Future close();

  public Future  done { get; }
  public Future addStream(Stream<S> stream) {
      throw new System.NotImplementedException();
  }

  public Future closeConsumer() {
      throw new System.NotImplementedException();
  }
}

public abstract class StreamTransformer<S, T> {
    // c# does not support change constructor
    public static StreamTransformer<S, T> create<S, T>(_async._SubscriptionTransformer<S, T> onListen) {
      return new _StreamSubscriptionTransformer<S, T>(onListen);  
    }
      

  public static StreamTransformer<S, T> fromHandlers(
      {void handleData(S data, EventSink<T> sink),
      void handleError(Object error, string stackTrace, EventSink<T> sink),
      void handleDone(EventSink<T> sink)}) = _StreamHandlerTransformer<S, T>;

  // @Since("2.1")
  factory StreamTransformer.fromBind(Stream<T> Function(Stream<S>) bind) =
      _StreamBindTransformer<S, T>;

  public static StreamTransformer<TS, TT> castFrom<SS, ST, TS, TT>(
      StreamTransformer<SS, ST> source) {
    return new CastStreamTransformer<SS, ST, TS, TT>(source);
  }

  public abstract Stream<T> bind(Stream<S> stream);

  public abstract StreamTransformer<RS, RT> cast<RS, RT>();
}

public abstract class StreamTransformerBase<S, T> : StreamTransformer<S, T> {
  public StreamTransformerBase() {}

 public override StreamTransformer<RS, RT> cast<RS, RT>() =>
      StreamTransformer<RS, RT>.castFrom<S, T, RS, RT>(this);
}

public abstract class StreamIterator<T> {
  /** Create a [StreamIterator] on [stream]. */
  public static  StreamIterator<T> Create(Stream<T> stream)
      // TODO(lrn): use redirecting factory constructor when type
      // arguments are supported.
      =>
      new _StreamIterator<T>(stream);

  public abstract Future<bool> moveNext();

  T  current { get; }

  public abstract Future cancel();
}

internal class _ControllerEventSinkWrapper<T> : EventSink<T> {
  EventSink<T> _sink;

  _ControllerEventSinkWrapper(EventSink<T> _sink) {
      this._sink = _sink;
  }

  public override void add(T data) {
    _sink.add(data);
  }

  public override void addError(object error, string stackTrace) {
    _sink.addError(error, stackTrace);
  }

  public override void close() {
    _sink.close();
  }
}

}