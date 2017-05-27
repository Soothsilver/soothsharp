(get-info :version)
; (:version "4.4.0")
; Input file is D:\TEMP\tmpC60B.tmp
; Started: 2017-05-27 08:00:56
; Silicon.buildVersion: 1.1-SNAPSHOT 0e750e485a3f default 2017/01/04 14:11:46
; ------------------------------------------------------------
; Preamble start
; 
; ; /z3config.smt2
(set-option :print-success true) ; Boogie: false
(set-option :global-decls true) ; Boogie: default
(set-option :auto_config false) ; Usually a good idea
(set-option :smt.mbqi false)
(set-option :model.v2 true)
(set-option :smt.phase_selection 0)
(set-option :smt.restart_strategy 0)
(set-option :smt.restart_factor |1.5|)
(set-option :smt.arith.random_initial_value true)
(set-option :smt.case_split 3)
(set-option :smt.delay_units true)
(set-option :smt.delay_units_threshold 16)
(set-option :nnf.sk_hack true)
(set-option :smt.qi.eager_threshold 100)
(set-option :smt.qi.cost "(+ weight generation)")
(set-option :type_check true)
(set-option :smt.bv.reflect true)
; 
; ; /preamble.smt2
(declare-datatypes () ((
    $Snap ($Snap.unit)
    ($Snap.combine ($Snap.first $Snap) ($Snap.second $Snap)))))
(declare-sort $Ref 0)
(declare-const $Ref.null $Ref)
(define-sort $Perm () Real)
(define-const $Perm.Write $Perm 1.0)
(define-const $Perm.No $Perm 0.0)
(define-fun $Perm.isValidVar ((p $Perm)) Bool
	(<= $Perm.No p))
(define-fun $Perm.isReadVar ((p $Perm) (ub $Perm)) Bool
    (and ($Perm.isValidVar p)
         (not (= p $Perm.No))
         (< p $Perm.Write)))
(define-fun $Perm.min ((p1 $Perm) (p2 $Perm)) Real
    (ite (<= p1 p2) p1 p2))
(define-fun $Math.min ((a Int) (b Int)) Int
    (ite (<= a b) a b))
(define-fun $Math.clip ((a Int)) Int
    (ite (< a 0) 0 a))
(declare-sort $Seq<Int>)
(declare-sort $FVF<$Ref>)
(declare-sort $Set<$Snap>)
(declare-sort $PSF<$Snap>)
; /dafny_axioms/sequences_declarations_dafny.smt2 [Int]
(declare-fun $Seq.length ($Seq<Int>) Int)
(declare-fun $Seq.empty<Int> () $Seq<Int>)
(declare-fun $Seq.singleton (Int) $Seq<Int>)
(declare-fun $Seq.build ($Seq<Int> Int) $Seq<Int>)
(declare-fun $Seq.index ($Seq<Int> Int) Int)
(declare-fun $Seq.append ($Seq<Int> $Seq<Int>) $Seq<Int>)
(declare-fun $Seq.update ($Seq<Int> Int Int) $Seq<Int>)
(declare-fun $Seq.contains ($Seq<Int> Int) Bool)
(declare-fun $Seq.take ($Seq<Int> Int) $Seq<Int>)
(declare-fun $Seq.drop ($Seq<Int> Int) $Seq<Int>)
(declare-fun $Seq.equal ($Seq<Int> $Seq<Int>) Bool)
(declare-fun $Seq.sameuntil ($Seq<Int> $Seq<Int> Int) Bool)
; /dafny_axioms/sequences_int_declarations_dafny.smt2
(declare-fun $Seq.range (Int Int) $Seq<Int>)
; /dafny_axioms/sets_declarations_dafny.smt2 [Snap
(declare-fun $Set.in ($Snap $Set<$Snap>) Bool)
(declare-fun $Set.card ($Set<$Snap>) Int)
(declare-fun $Set.empty<$Snap> () $Set<$Snap>)
(declare-fun $Set.singleton ($Snap) $Set<$Snap>)
(declare-fun $Set.unionone ($Set<$Snap> $Snap) $Set<$Snap>)
(declare-fun $Set.union ($Set<$Snap> $Set<$Snap>) $Set<$Snap>)
(declare-fun $Set.disjoint ($Set<$Snap> $Set<$Snap>) Bool)
(declare-fun $Set.difference ($Set<$Snap> $Set<$Snap>) $Set<$Snap>)
(declare-fun $Set.intersection ($Set<$Snap> $Set<$Snap>) $Set<$Snap>)
(declare-fun $Set.subset ($Set<$Snap> $Set<$Snap>) Bool)
(declare-fun $Set.equal ($Set<$Snap> $Set<$Snap>) Bool)
; Declaring program functions
(declare-fun arrayRead ($Snap $Ref Int) Int)
(declare-fun arrayRead%limited ($Snap $Ref Int) Int)
(declare-fun arrayRead%stateless ($Ref Int) Bool)
; Snapshot variable to be used during function verification
(declare-fun s@$ () $Snap)
; Declaring predicate trigger functions
; /dafny_axioms/sequences_axioms_dafny.smt2 [Int]
(assert (forall ((s $Seq<Int>) ) (! (<= 0 ($Seq.length s))
  :pattern ( ($Seq.length s))
  )))
(assert (= ($Seq.length $Seq.empty<Int>) 0))
(assert (forall ((s $Seq<Int>) ) (! (=> (= ($Seq.length s) 0) (= s $Seq.empty<Int>))
  :pattern ( ($Seq.length s))
  )))
(assert (forall ((t Int) ) (! (= ($Seq.length ($Seq.singleton t)) 1)
  :pattern ( ($Seq.length ($Seq.singleton t)))
  )))
(assert (forall ((s $Seq<Int>) (v Int) ) (! (= ($Seq.length ($Seq.build s v)) (+ 1 ($Seq.length s)))
  :pattern ( ($Seq.length ($Seq.build s v)))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) ) (! (and
  (=> (= i ($Seq.length s)) (= ($Seq.index ($Seq.build s v) i) v))
  (=> (not (= i ($Seq.length s))) (= ($Seq.index ($Seq.build s v) i) ($Seq.index s i))))
  :pattern ( ($Seq.index ($Seq.build s v) i))
  )))
(assert (forall ((s0 $Seq<Int>) (s1 $Seq<Int>) ) (!
  (implies ; The implication was not in the Dafny version
    (and
      (not (= s0 $Seq.empty<Int>))
      (not (= s1 $Seq.empty<Int>)))
    (=
      ($Seq.length ($Seq.append s0 s1))
      (+ ($Seq.length s0) ($Seq.length s1))))
  :pattern ( ($Seq.length ($Seq.append s0 s1)))
  )))
(assert (forall ((t Int) ) (! (= ($Seq.index ($Seq.singleton t) 0) t)
  :pattern ( ($Seq.index ($Seq.singleton t) 0))
  )))
(assert (forall ((s $Seq<Int>)) (! ; The axiom was not in the Dafny version
  (= ($Seq.append s $Seq.empty<Int>) s)
  :pattern (($Seq.append s $Seq.empty<Int>))
  )))
(assert (forall ((s $Seq<Int>)) (! ; The axiom was not in the Dafny version
  (= ($Seq.append $Seq.empty<Int> s) s)
  :pattern (($Seq.append $Seq.empty<Int> s))
  )))
(assert (forall ((s0 $Seq<Int>) (s1 $Seq<Int>) (n Int) ) (!
  (implies ; The implication was not in the Dafny version
    (and
      (not (= s0 $Seq.empty<Int>))
      (not (= s1 $Seq.empty<Int>)))
    (and
      (=> (< n ($Seq.length s0)) (= ($Seq.index ($Seq.append s0 s1) n) ($Seq.index s0 n)))
      (=> (<= ($Seq.length s0) n) (= ($Seq.index ($Seq.append s0 s1) n) ($Seq.index s1 (- n ($Seq.length s0)))))))
  :pattern ( ($Seq.index ($Seq.append s0 s1) n))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) ) (! (=> (and
  (<= 0 i)
  (< i ($Seq.length s))) (= ($Seq.length ($Seq.update s i v)) ($Seq.length s)))
  :pattern ( ($Seq.length ($Seq.update s i v)))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) (n Int) ) (! (=> (and
  (<= 0 n)
  (< n ($Seq.length s))) (and
  (=> (= i n) (= ($Seq.index ($Seq.update s i v) n) v))
  (=> (not (= i n)) (= ($Seq.index ($Seq.update s i v) n) ($Seq.index s n)))))
  :pattern ( ($Seq.index ($Seq.update s i v) n))
  )))
(assert (forall ((s $Seq<Int>) (x Int) ) (!
  (and
    (=>
      ($Seq.contains s x)
      (exists ((i Int) ) (!
        (and
  (<= 0 i)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
  )))
    (=>
      (exists ((i Int) ) (!
        (and
  (<= 0 i)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
      ))
      ($Seq.contains s x)))
  :pattern ( ($Seq.contains s x))
  )))
(assert (forall ((x Int) ) (! (not ($Seq.contains $Seq.empty<Int> x))
  :pattern ( ($Seq.contains $Seq.empty<Int> x))
  )))
(assert (forall ((s0 $Seq<Int>) (s1 $Seq<Int>) (x Int) ) (! (and
  (=> ($Seq.contains ($Seq.append s0 s1) x) (or
  ($Seq.contains s0 x)
  ($Seq.contains s1 x)))
  (=> (or
  ($Seq.contains s0 x)
  ($Seq.contains s1 x)) ($Seq.contains ($Seq.append s0 s1) x)))
  :pattern ( ($Seq.contains ($Seq.append s0 s1) x))
  )))
(assert (forall ((s $Seq<Int>) (v Int) (x Int) ) (! (and
  (=> ($Seq.contains ($Seq.build s v) x) (or
  (= v x)
  ($Seq.contains s x)))
  (=> (or
  (= v x)
  ($Seq.contains s x)) ($Seq.contains ($Seq.build s v) x)))
  :pattern ( ($Seq.contains ($Seq.build s v) x))
  )))
(assert (forall ((s $Seq<Int>) (n Int) (x Int) ) (!
  (and
    (=>
      ($Seq.contains ($Seq.take s n) x)
      (exists ((i Int) ) (!
        (and
  (<= 0 i)
  (< i n)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
  )))
    (=>
      (exists ((i Int) ) (!
        (and
  (<= 0 i)
  (< i n)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
      ))
      ($Seq.contains ($Seq.take s n) x)))
  :pattern ( ($Seq.contains ($Seq.take s n) x))
  )))
(assert (forall ((s $Seq<Int>) (n Int) (x Int) ) (!
  (and
    (=>
      ($Seq.contains ($Seq.drop s n) x)
      (exists ((i Int) ) (!
        (and
  (<= 0 n)
  (<= n i)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
  )))
    (=>
      (exists ((i Int) ) (!
        (and
  (<= 0 n)
  (<= n i)
  (< i ($Seq.length s))
  (= ($Seq.index s i) x))
  :pattern ( ($Seq.index s i))
      ))
      ($Seq.contains ($Seq.drop s n) x)))
  :pattern ( ($Seq.contains ($Seq.drop s n) x))
  )))
(assert (forall ((s0 $Seq<Int>) (s1 $Seq<Int>) ) (! (and
  (=> ($Seq.equal s0 s1) (and
  (= ($Seq.length s0) ($Seq.length s1))
  (forall ((j Int) ) (! (=> (and
  (<= 0 j)
  (< j ($Seq.length s0))) (= ($Seq.index s0 j) ($Seq.index s1 j)))
  :pattern ( ($Seq.index s0 j))
  :pattern ( ($Seq.index s1 j))
  ))))
  (=> (and
  (= ($Seq.length s0) ($Seq.length s1))
  (forall ((j Int) ) (! (=> (and
  (<= 0 j)
  (< j ($Seq.length s0))) (= ($Seq.index s0 j) ($Seq.index s1 j)))
  :pattern ( ($Seq.index s0 j))
  :pattern ( ($Seq.index s1 j))
  ))) ($Seq.equal s0 s1)))
  :pattern ( ($Seq.equal s0 s1))
  )))
(assert (forall ((a $Seq<Int>) (b $Seq<Int>) ) (! (=> ($Seq.equal a b) (= a b))
  :pattern ( ($Seq.equal a b))
  )))
(assert (forall ((s0 $Seq<Int>) (s1 $Seq<Int>) (n Int) ) (! (and
  (=> ($Seq.sameuntil s0 s1 n) (forall ((j Int) ) (! (=> (and
  (<= 0 j)
  (< j n)) (= ($Seq.index s0 j) ($Seq.index s1 j)))
  :pattern ( ($Seq.index s0 j))
  :pattern ( ($Seq.index s1 j))
  )))
  (=> (forall ((j Int) ) (! (=> (and
  (<= 0 j)
  (< j n)) (= ($Seq.index s0 j) ($Seq.index s1 j)))
  :pattern ( ($Seq.index s0 j))
  :pattern ( ($Seq.index s1 j))
  )) ($Seq.sameuntil s0 s1 n)))
  :pattern ( ($Seq.sameuntil s0 s1 n))
  )))
(assert (forall ((s $Seq<Int>) (n Int) ) (! (=> (<= 0 n) (and
  (=> (<= n ($Seq.length s)) (= ($Seq.length ($Seq.take s n)) n))
  (=> (< ($Seq.length s) n) (= ($Seq.length ($Seq.take s n)) ($Seq.length s)))))
  :pattern ( ($Seq.length ($Seq.take s n)))
  )))
(assert (forall ((s $Seq<Int>) (n Int) (j Int) ) (! (=> (and
  (<= 0 j)
  (< j n)
  (< j ($Seq.length s))) (= ($Seq.index ($Seq.take s n) j) ($Seq.index s j)))
  :pattern ( ($Seq.index ($Seq.take s n) j))
  :pattern (($Seq.index s j) ($Seq.take s n)) ; [XXX] Added 29-10-2015
  )))
(assert (forall ((s $Seq<Int>) (n Int) ) (! (=> (<= 0 n) (and
  (=> (<= n ($Seq.length s)) (= ($Seq.length ($Seq.drop s n)) (- ($Seq.length s) n)))
  (=> (< ($Seq.length s) n) (= ($Seq.length ($Seq.drop s n)) 0))))
  :pattern ( ($Seq.length ($Seq.drop s n)))
  )))
(assert (forall ((s $Seq<Int>) (n Int) (j Int) ) (! (=> (and
  (<= 0 n)
  (<= 0 j)
  (< j (- ($Seq.length s) n))) (= ($Seq.index ($Seq.drop s n) j) ($Seq.index s (+ j n))))
  :pattern ( ($Seq.index ($Seq.drop s n) j))
  )))
(assert (forall ((s $Seq<Int>) (n Int) (k Int) ) (! ; [XXX] Added 29-10-2015
  (=>
    (and
      (<= 0 n)
      (<= n k)
      (< k ($Seq.length s)))
    (=
      ($Seq.index ($Seq.drop s n) (- k n))
      ($Seq.index s k)))
  :pattern (($Seq.index s k) ($Seq.drop s n))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) (n Int) ) (! (=> (and
  (<= 0 i)
  (< i n)
  (<= n ($Seq.length s))) (= ($Seq.take ($Seq.update s i v) n) ($Seq.update ($Seq.take s n) i v)))
  :pattern ( ($Seq.take ($Seq.update s i v) n))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) (n Int) ) (! (=> (and
  (<= n i)
  (< i ($Seq.length s))) (= ($Seq.take ($Seq.update s i v) n) ($Seq.take s n)))
  :pattern ( ($Seq.take ($Seq.update s i v) n))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) (n Int) ) (! (=> (and
  (<= 0 n)
  (<= n i)
  (< i ($Seq.length s))) (= ($Seq.drop ($Seq.update s i v) n) ($Seq.update ($Seq.drop s n) (- i n) v)))
  :pattern ( ($Seq.drop ($Seq.update s i v) n))
  )))
(assert (forall ((s $Seq<Int>) (i Int) (v Int) (n Int) ) (! (=> (and
  (<= 0 i)
  (< i n)
  (< n ($Seq.length s))) (= ($Seq.drop ($Seq.update s i v) n) ($Seq.drop s n)))
  :pattern ( ($Seq.drop ($Seq.update s i v) n))
  )))
(assert (forall ((s $Seq<Int>) (v Int) (n Int) ) (! (=> (and
  (<= 0 n)
  (<= n ($Seq.length s))) (= ($Seq.drop ($Seq.build s v) n) ($Seq.build ($Seq.drop s n) v)))
  :pattern ( ($Seq.drop ($Seq.build s v) n))
  )))
(assert (forall ((x Int) (y Int)) (!
  (iff
    ($Seq.contains ($Seq.singleton x) y)
    (= x y))
  :pattern (($Seq.contains ($Seq.singleton x) y))
  )))
; /dafny_axioms/sequences_int_axioms_dafny.smt2
(assert (forall ((min Int) (max Int) ) (! (and
  (=> (< min max) (= ($Seq.length ($Seq.range min max)) (- max min)))
  (=> (<= max min) (= ($Seq.length ($Seq.range min max)) 0)))
   :pattern ( ($Seq.length ($Seq.range min max)))
  )))
(assert (forall ((min Int) (max Int) (j Int) ) (! (=> (and
  (<= 0 j)
  (< j (- max min))) (= ($Seq.index ($Seq.range min max) j) (+ min j)))
   :pattern ( ($Seq.index ($Seq.range min max) j))
  )))
(assert (forall ((min Int) (max Int) (v Int) ) (! (and
  (=> ($Seq.contains ($Seq.range min max) v) (and
  (<= min v)
  (< v max)))
  (=> (and
  (<= min v)
  (< v max)) ($Seq.contains ($Seq.range min max) v)))
   :pattern ( ($Seq.contains ($Seq.range min max) v))
  )))
; Declaring additional sort wrappers
(declare-fun $SortWrappers.$PermTo$Snap ($Perm) $Snap)
(declare-fun $SortWrappers.$SnapTo$Perm ($Snap) $Perm)
(assert (forall ((x $Perm)) (!
    (= x ($SortWrappers.$SnapTo$Perm($SortWrappers.$PermTo$Snap x)))
    :pattern (($SortWrappers.$PermTo$Snap x))
    :qid |$Snap.$Perm|
    )))
(declare-fun $SortWrappers.$RefTo$Snap ($Ref) $Snap)
(declare-fun $SortWrappers.$SnapTo$Ref ($Snap) $Ref)
(assert (forall ((x $Ref)) (!
    (= x ($SortWrappers.$SnapTo$Ref($SortWrappers.$RefTo$Snap x)))
    :pattern (($SortWrappers.$RefTo$Snap x))
    :qid |$Snap.$Ref|
    )))
(declare-fun $SortWrappers.BoolTo$Snap (Bool) $Snap)
(declare-fun $SortWrappers.$SnapToBool ($Snap) Bool)
(assert (forall ((x Bool)) (!
    (= x ($SortWrappers.$SnapToBool($SortWrappers.BoolTo$Snap x)))
    :pattern (($SortWrappers.BoolTo$Snap x))
    :qid |$Snap.Bool|
    )))
(declare-fun $SortWrappers.IntTo$Snap (Int) $Snap)
(declare-fun $SortWrappers.$SnapToInt ($Snap) Int)
(assert (forall ((x Int)) (!
    (= x ($SortWrappers.$SnapToInt($SortWrappers.IntTo$Snap x)))
    :pattern (($SortWrappers.IntTo$Snap x))
    :qid |$Snap.Int|
    )))
; Declaring additional sort wrappers
(declare-fun $SortWrappers.$Seq<Int>To$Snap ($Seq<Int>) $Snap)
(declare-fun $SortWrappers.$SnapTo$Seq<Int> ($Snap) $Seq<Int>)
(assert (forall ((x $Seq<Int>)) (!
    (= x ($SortWrappers.$SnapTo$Seq<Int>($SortWrappers.$Seq<Int>To$Snap x)))
    :pattern (($SortWrappers.$Seq<Int>To$Snap x))
    :qid |$Snap.$Seq<Int>|
    )))
; Declaring additional sort wrappers
(declare-fun $SortWrappers.$FVF<$Ref>To$Snap ($FVF<$Ref>) $Snap)
(declare-fun $SortWrappers.$SnapTo$FVF<$Ref> ($Snap) $FVF<$Ref>)
(assert (forall ((x $FVF<$Ref>)) (!
    (= x ($SortWrappers.$SnapTo$FVF<$Ref>($SortWrappers.$FVF<$Ref>To$Snap x)))
    :pattern (($SortWrappers.$FVF<$Ref>To$Snap x))
    :qid |$Snap.$FVF<$Ref>|
    )))
; Declaring additional sort wrappers
(declare-fun $SortWrappers.$PSF<$Snap>To$Snap ($PSF<$Snap>) $Snap)
(declare-fun $SortWrappers.$SnapTo$PSF<$Snap> ($Snap) $PSF<$Snap>)
(assert (forall ((x $PSF<$Snap>)) (!
    (= x ($SortWrappers.$SnapTo$PSF<$Snap>($SortWrappers.$PSF<$Snap>To$Snap x)))
    :pattern (($SortWrappers.$PSF<$Snap>To$Snap x))
    :qid |$Snap.$PSF<$Snap>|
    )))
; Preamble end
; ------------------------------------------------------------
; ---------- FUNCTION arrayRead----------
(declare-fun array@0 () $Ref)
(declare-fun index@1 () Int)
(declare-fun result@2 () Int)
; ----- Well-definedness of specifications -----
(push) ; 1
(declare-const $k@3 $Perm)
(assert ($Perm.isReadVar $k@3 $Perm.Write))
(assert (<= $Perm.No $k@3))
(assert (implies (< $Perm.No $k@3) (not (= array@0 $Ref.null))))
(assert (= ($Snap.second s@$) $Snap.unit))
; [eval] |array.arrayContents| > index
; [eval] |array.arrayContents|
(set-option :timeout 0)
(push) ; 2
(assert (not (not (= $k@3 $Perm.No))))
(check-sat)
; unsat
(pop) ; 2
; 0,00s
; (get-info :all-statistics)
(assert (not (= $k@3 $Perm.No)))
(assert (> ($Seq.length ($SortWrappers.$SnapTo$Seq<Int> ($Snap.first s@$))) index@1))
(pop) ; 1
(assert (forall ((s@$ $Snap) (array@0 $Ref) (index@1 Int)) (!
  (= (arrayRead%limited s@$ array@0 index@1) (arrayRead s@$ array@0 index@1))
  :pattern ((arrayRead s@$ array@0 index@1))
  )))
(assert (forall ((s@$ $Snap) (array@0 $Ref) (index@1 Int)) (!
  (arrayRead%stateless array@0 index@1)
  :pattern ((arrayRead%limited s@$ array@0 index@1))
  )))
; ---------- FUNCTION arrayRead (verify) ----------
(push) ; 1
(assert (> ($Seq.length ($SortWrappers.$SnapTo$Seq<Int> ($Snap.first s@$))) index@1))
(assert (not (= $k@3 $Perm.No)))
(assert (= ($Snap.second s@$) $Snap.unit))
(assert (implies (< $Perm.No $k@3) (not (= array@0 $Ref.null))))
(assert (<= $Perm.No $k@3))
(assert ($Perm.isReadVar $k@3 $Perm.Write))
; [eval] array.arrayContents[index]
(assert (=
  result@2
  ($Seq.index ($SortWrappers.$SnapTo$Seq<Int> ($Snap.first s@$)) index@1)))
(pop) ; 1
(assert (forall ((s@$ $Snap) (array@0 $Ref) (index@1 Int)) (!
  (let ((result@2 (arrayRead%limited s@$ array@0 index@1))) (implies
    (> ($Seq.length ($SortWrappers.$SnapTo$Seq<Int> ($Snap.first s@$))) index@1)
    (=
      (arrayRead s@$ array@0 index@1)
      ($Seq.index ($SortWrappers.$SnapTo$Seq<Int> ($Snap.first s@$)) index@1))))
  :pattern ((arrayRead s@$ array@0 index@1))
  )))
; ---------- ForLoop_a ----------
(declare-const this@4 $Ref)
(declare-const res@5 Int)
(declare-const result2@6 Int)
(declare-const a@7 Int)
(push) ; 1
(push) ; 2
(declare-const $t@8 $Snap)
(assert (= $t@8 $Snap.unit))
; [eval] res == 1 + 2 + 3 + 4
; [eval] 1 + 2 + 3 + 4
; [eval] 1 + 2 + 3
; [eval] 1 + 2
(assert (= res@5 10))
(pop) ; 2
(push) ; 2
; [exec]
; result2 := 0
; [exec]
; a := 0
; loop at tmpC60B.tmp@8.2
(declare-const a@9 Int)
(declare-const result2@10 Int)
(push) ; 3
; Loop: Check specs well-definedness
(declare-const $t@11 $Snap)
(assert (= $t@11 $Snap.unit))
; [eval] result2 == (1 + a) * a \ 2
; [eval] (1 + a) * a \ 2
; [eval] (1 + a) * a
; [eval] 1 + a
(push) ; 4
(assert (not (not (= 2 0))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (= result2@10 (div (* (+ 1 a@9) a@9) 2)))
(declare-const $t@12 $Snap)
(assert (= $t@12 $Snap.unit))
; [eval] a != 4
(assert (not (= a@9 4)))
(set-option :timeout 250)
(check-sat)
; unknown
(pop) ; 3
(push) ; 3
; Loop: Establish loop invariant
; [eval] result2 == (1 + a) * a \ 2
; [eval] (1 + a) * a \ 2
; [eval] (1 + a) * a
; [eval] 1 + a
(set-option :timeout 0)
(push) ; 4
(assert (not (not (= 2 0))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (= 0 (div 0 2))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (= 0 (div 0 2)))
(pop) ; 3
; Loop: Verify loop body
(push) ; 3
(assert (not (= a@9 4)))
(assert (= $t@12 $Snap.unit))
(assert (= result2@10 (div (* (+ 1 a@9) a@9) 2)))
(assert (= $t@11 $Snap.unit))
; [exec]
; result2 := result2 + (a + 1)
; [eval] result2 + (a + 1)
; [eval] a + 1
(declare-const result2@13 Int)
(assert (= result2@13 (+ result2@10 (+ a@9 1))))
; [exec]
; a := a + 1
; [eval] a + 1
(declare-const a@14 Int)
(assert (= a@14 (+ a@9 1)))
; [eval] result2 == (1 + a) * a \ 2
; [eval] (1 + a) * a \ 2
; [eval] (1 + a) * a
; [eval] 1 + a
(push) ; 4
(assert (not (not (= 2 0))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (= result2@13 (div (* (+ 1 a@14) a@14) 2))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (= result2@13 (div (* (+ 1 a@14) a@14) 2)))
(pop) ; 3
; Loop: Continue after loop
(push) ; 3
(assert (= 0 (div 0 2)))
(declare-const $t@15 $Snap)
(assert (= $t@15 ($Snap.combine $Snap.unit $Snap.unit)))
; [eval] result2 == (1 + a) * a \ 2
; [eval] (1 + a) * a \ 2
; [eval] (1 + a) * a
; [eval] 1 + a
(push) ; 4
(assert (not (not (= 2 0))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (= result2@10 (div (* (+ 1 a@9) a@9) 2)))
; [eval] !(a != 4)
; [eval] a != 4
(assert (= a@9 4))
(set-option :timeout 250)
(check-sat)
; unknown
; [exec]
; res := result2
; [eval] res == 1 + 2 + 3 + 4
; [eval] 1 + 2 + 3 + 4
; [eval] 1 + 2 + 3
; [eval] 1 + 2
(set-option :timeout 0)
(push) ; 4
(assert (not (= result2@10 10)))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (= result2@10 10))
(pop) ; 3
(pop) ; 2
(pop) ; 1
; ---------- ForLoop_init ----------
(declare-const this@16 $Ref)
(push) ; 1
(push) ; 2
(pop) ; 2
(push) ; 2
; [exec]
; this := new(arrayContents)
(declare-const this@17 $Ref)
(assert (not (= this@17 $Ref.null)))
(declare-const arrayContents@18 $Seq<Int>)
(pop) ; 2
(pop) ; 1
