(get-info :version)
; (:version "4.4.1")
; Input file is C:\Users\Soothsilver\AppData\Local\Temp\tmp2078.tmp
; Started: 2017-05-17 17:19:14
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
; ---------- Arithmetic_Max ----------
(declare-const a@0 Int)
(declare-const b@1 Int)
(declare-const res@2 Int)
(push) ; 1
(push) ; 2
(declare-const $t@3 $Snap)
(assert (= $t@3 $Snap.unit))
; [eval] (a > b ? res == a : res == b)
; [eval] a > b
(push) ; 3
(set-option :timeout 250)
(push) ; 4
(assert (not (not (> a@0 b@1))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (> a@0 b@1)))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
; [then-branch 0] a@0 > b@1
(assert (> a@0 b@1))
; [eval] res == a
(pop) ; 4
(push) ; 4
; [else-branch 0] !a@0 > b@1
(assert (not (> a@0 b@1)))
; [eval] res == b
(pop) ; 4
(pop) ; 3
; Joined path conditions
; Joined path conditions
(assert (ite (> a@0 b@1) (= res@2 a@0) (= res@2 b@1)))
(pop) ; 2
(push) ; 2
; [eval] a < b
(push) ; 3
(assert (not (not (< a@0 b@1))))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
(assert (not (< a@0 b@1)))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
; [then-branch 1] a@0 < b@1
(assert (< a@0 b@1))
; [exec]
; res := b
; [exec]
; label end
; [eval] (a > b ? res == a : res == b)
; [eval] a > b
(push) ; 4
(push) ; 5
(assert (not (not (> a@0 b@1))))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
; [dead then-branch 2] a@0 > b@1
(push) ; 5
; [else-branch 2] !a@0 > b@1
(assert (not (> a@0 b@1)))
; [eval] res == b
(pop) ; 5
(pop) ; 4
; Joined path conditions
(declare-const $deadThen@4 Bool)
(set-option :timeout 0)
(push) ; 4
(assert (not (ite (> a@0 b@1) $deadThen@4 true)))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (ite (> a@0 b@1) $deadThen@4 true))
(pop) ; 3
(push) ; 3
; [else-branch 1] !a@0 < b@1
(assert (not (< a@0 b@1)))
(pop) ; 3
; [eval] !(a < b)
; [eval] a < b
(set-option :timeout 250)
(push) ; 3
(assert (not (< a@0 b@1)))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
(assert (not (not (< a@0 b@1))))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
; [then-branch 3] !a@0 < b@1
(assert (not (< a@0 b@1)))
; [exec]
; res := a
; [exec]
; label end
; [eval] (a > b ? res == a : res == b)
; [eval] a > b
(push) ; 4
(push) ; 5
(assert (not (not (> a@0 b@1))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (> a@0 b@1)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 4] a@0 > b@1
(assert (> a@0 b@1))
; [eval] res == a
(pop) ; 5
(push) ; 5
; [else-branch 4] !a@0 > b@1
(assert (not (> a@0 b@1)))
; [eval] res == b
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(set-option :timeout 0)
(push) ; 4
(assert (not (ite (> a@0 b@1) true (= a@0 b@1))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (ite (> a@0 b@1) true (= a@0 b@1)))
(pop) ; 3
(push) ; 3
; [else-branch 3] a@0 < b@1
(assert (< a@0 b@1))
(pop) ; 3
(pop) ; 2
(pop) ; 1
; ---------- Arithmetic_Min ----------
(declare-const a2@5 Int)
(declare-const b2@6 Int)
(declare-const res@7 Int)
(declare-const max@8 Int)
(declare-const _tmp1@9 Int)
(push) ; 1
(push) ; 2
(declare-const $t@10 $Snap)
(assert (= $t@10 $Snap.unit))
; [eval] (a2 < b2 ? res == a2 : res == b2)
; [eval] a2 < b2
(push) ; 3
(set-option :timeout 250)
(push) ; 4
(assert (not (not (< a2@5 b2@6))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (< a2@5 b2@6)))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
; [then-branch 5] a2@5 < b2@6
(assert (< a2@5 b2@6))
; [eval] res == a2
(pop) ; 4
(push) ; 4
; [else-branch 5] !a2@5 < b2@6
(assert (not (< a2@5 b2@6)))
; [eval] res == b2
(pop) ; 4
(pop) ; 3
; Joined path conditions
; Joined path conditions
(assert (ite (< a2@5 b2@6) (= res@7 a2@5) (= res@7 b2@6)))
(pop) ; 2
(push) ; 2
; [exec]
; _tmp1 := Arithmetic_Max(a2, b2)
(declare-const res@11 Int)
(declare-const $t@12 $Snap)
(assert (= $t@12 $Snap.unit))
; [eval] (a > b ? res == a : res == b)
; [eval] a > b
(push) ; 3
(push) ; 4
(assert (not (not (> a2@5 b2@6))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (> a2@5 b2@6)))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
; [then-branch 6] a2@5 > b2@6
(assert (> a2@5 b2@6))
; [eval] res == a
(pop) ; 4
(push) ; 4
; [else-branch 6] !a2@5 > b2@6
(assert (not (> a2@5 b2@6)))
; [eval] res == b
(pop) ; 4
(pop) ; 3
; Joined path conditions
; Joined path conditions
(assert (ite (> a2@5 b2@6) (= res@11 a2@5) (= res@11 b2@6)))
; [exec]
; max := _tmp1
; [eval] a2 == max
(push) ; 3
(assert (not (not (= a2@5 res@11))))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
(assert (not (= a2@5 res@11)))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
; [then-branch 7] a2@5 == res@11
(assert (= a2@5 res@11))
; [exec]
; res := b2
; [exec]
; label end
; [eval] (a2 < b2 ? res == a2 : res == b2)
; [eval] a2 < b2
(push) ; 4
(push) ; 5
(assert (not (not (< a2@5 b2@6))))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
; [dead then-branch 8] a2@5 < b2@6
(push) ; 5
; [else-branch 8] !a2@5 < b2@6
(assert (not (< a2@5 b2@6)))
; [eval] res == b2
(pop) ; 5
(pop) ; 4
; Joined path conditions
(declare-const $deadThen@13 Bool)
(set-option :timeout 0)
(push) ; 4
(assert (not (ite (< a2@5 b2@6) $deadThen@13 true)))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (ite (< a2@5 b2@6) $deadThen@13 true))
(pop) ; 3
(push) ; 3
; [else-branch 7] a2@5 != res@11
(assert (not (= a2@5 res@11)))
(pop) ; 3
; [eval] !(a2 == max)
; [eval] a2 == max
(set-option :timeout 250)
(push) ; 3
(assert (not (= a2@5 res@11)))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
(assert (not (not (= a2@5 res@11))))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
; [then-branch 9] a2@5 != res@11
(assert (not (= a2@5 res@11)))
; [exec]
; res := a2
; [exec]
; label end
; [eval] (a2 < b2 ? res == a2 : res == b2)
; [eval] a2 < b2
(push) ; 4
(push) ; 5
(assert (not (not (< a2@5 b2@6))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (< a2@5 b2@6)))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 10] a2@5 < b2@6
(assert (< a2@5 b2@6))
; [eval] res == a2
(pop) ; 5
; [dead else-branch 10] !a2@5 < b2@6
(pop) ; 4
; Joined path conditions
(declare-const $deadElse@14 Bool)
(set-option :timeout 0)
(push) ; 4
(assert (not (ite (< a2@5 b2@6) true $deadElse@14)))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (ite (< a2@5 b2@6) true $deadElse@14))
(pop) ; 3
(push) ; 3
; [else-branch 9] a2@5 == res@11
(assert (= a2@5 res@11))
(pop) ; 3
(pop) ; 2
(pop) ; 1
; ---------- Search_BinarySearch ----------
(declare-const xs@15 $Seq<Int>)
(declare-const key@16 Int)
(declare-const res@17 Int)
(declare-const low@18 Int)
(declare-const high@19 Int)
(declare-const index@20 Int)
(push) ; 1
(declare-const $t@21 $Snap)
(assert (= $t@21 $Snap.unit))
; [eval] (forall i: Int :: (forall j: Int :: 0 <= i && (j < |xs| && i < j) ==> xs[i] < xs[j]))
(declare-const i@22 Int)
(push) ; 2
; [eval] (forall j: Int :: 0 <= i && (j < |xs| && i < j) ==> xs[i] < xs[j])
(declare-const j@23 Int)
(push) ; 3
; [eval] 0 <= i && (j < |xs| && i < j) ==> xs[i] < xs[j]
; [eval] 0 <= i && (j < |xs| && i < j)
; [eval] 0 <= i
; [eval] 0 <= i ==> j < |xs| && i < j
; [eval] 0 <= i
(push) ; 4
(set-option :timeout 250)
(push) ; 5
(assert (not (not (<= 0 i@22))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 i@22)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 11] 0 <= i@22
(assert (<= 0 i@22))
; [eval] j < |xs| && i < j
; [eval] j < |xs|
; [eval] |xs|
; [eval] j < |xs| ==> i < j
; [eval] j < |xs|
; [eval] |xs|
(push) ; 6
(push) ; 7
(assert (not (not (< j@23 ($Seq.length xs@15)))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (< j@23 ($Seq.length xs@15))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 12] j@23 < |xs@15|
(assert (< j@23 ($Seq.length xs@15)))
; [eval] i < j
(pop) ; 7
(push) ; 7
; [else-branch 12] !j@23 < |xs@15|
(assert (not (< j@23 ($Seq.length xs@15))))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(pop) ; 5
(push) ; 5
; [else-branch 11] !0 <= i@22
(assert (not (<= 0 i@22)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(push) ; 4
(push) ; 5
(assert (not (not
  (and
    (<= 0 i@22)
    (implies
      (<= 0 i@22)
      (and
        (< j@23 ($Seq.length xs@15))
        (implies (< j@23 ($Seq.length xs@15)) (< i@22 j@23))))))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (and
  (<= 0 i@22)
  (implies
    (<= 0 i@22)
    (and
      (< j@23 ($Seq.length xs@15))
      (implies (< j@23 ($Seq.length xs@15)) (< i@22 j@23)))))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 13] 0 <= i@22 && 0 <= i@22 ==> j@23 < |xs@15| && j@23 < |xs@15| ==> i@22 < j@23
(assert (and
  (<= 0 i@22)
  (implies
    (<= 0 i@22)
    (and
      (< j@23 ($Seq.length xs@15))
      (implies (< j@23 ($Seq.length xs@15)) (< i@22 j@23))))))
; [eval] xs[i] < xs[j]
; [eval] xs[i]
; [eval] xs[j]
(pop) ; 5
(push) ; 5
; [else-branch 13] !0 <= i@22 && 0 <= i@22 ==> j@23 < |xs@15| && j@23 < |xs@15| ==> i@22 < j@23
(assert (not
  (and
    (<= 0 i@22)
    (implies
      (<= 0 i@22)
      (and
        (< j@23 ($Seq.length xs@15))
        (implies (< j@23 ($Seq.length xs@15)) (< i@22 j@23)))))))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(pop) ; 3
; Nested auxiliary terms
(pop) ; 2
; Nested auxiliary terms
(assert (forall ((i@22 Int)) (!
  (forall ((j@23 Int)) (!
    (implies
      (and
        (<= 0 i@22)
        (implies
          (<= 0 i@22)
          (and
            (< j@23 ($Seq.length xs@15))
            (implies (< j@23 ($Seq.length xs@15)) (< i@22 j@23)))))
      (< ($Seq.index xs@15 i@22) ($Seq.index xs@15 j@23)))
    :pattern (($Seq.index xs@15 j@23))
    :qid |prog.l30|))
  :pattern (($Seq.index xs@15 i@22))
  :qid |prog.l30|)))
(push) ; 2
(declare-const $t@24 $Snap)
(assert (= $t@24 ($Snap.combine $Snap.unit $Snap.unit)))
; [eval] -1 <= res && res < |xs|
; [eval] -1 <= res
; [eval] -1
; [eval] -1 <= res ==> res < |xs|
; [eval] -1 <= res
; [eval] -1
(push) ; 3
(push) ; 4
(assert (not (not (<= (- 0 1) res@17))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (<= (- 0 1) res@17)))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
; [then-branch 14] -1 <= res@17
(assert (<= (- 0 1) res@17))
; [eval] res < |xs|
; [eval] |xs|
(pop) ; 4
(push) ; 4
; [else-branch 14] !-1 <= res@17
(assert (not (<= (- 0 1) res@17)))
(pop) ; 4
(pop) ; 3
; Joined path conditions
; Joined path conditions
(assert (and
  (<= (- 0 1) res@17)
  (implies (<= (- 0 1) res@17) (< res@17 ($Seq.length xs@15)))))
; [eval] 0 <= res ==> xs[res] == key
; [eval] 0 <= res
(push) ; 3
(push) ; 4
(assert (not (not (<= 0 res@17))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (<= 0 res@17)))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
; [then-branch 15] 0 <= res@17
(assert (<= 0 res@17))
; [eval] xs[res] == key
; [eval] xs[res]
(pop) ; 4
(push) ; 4
; [else-branch 15] !0 <= res@17
(assert (not (<= 0 res@17)))
(pop) ; 4
(pop) ; 3
; Joined path conditions
; Joined path conditions
(assert (implies (<= 0 res@17) (= ($Seq.index xs@15 res@17) key@16)))
; [eval] -1 == res ==> (forall i2: Int :: 0 <= i2 && i2 < |xs| ==> xs[i2] != key)
; [eval] -1 == res
; [eval] -1
(push) ; 3
(push) ; 4
(assert (not (not (= (- 0 1) res@17))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (= (- 0 1) res@17)))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
; [then-branch 16] -1 == res@17
(assert (= (- 0 1) res@17))
; [eval] (forall i2: Int :: 0 <= i2 && i2 < |xs| ==> xs[i2] != key)
(declare-const i2@25 Int)
(push) ; 5
; [eval] 0 <= i2 && i2 < |xs| ==> xs[i2] != key
; [eval] 0 <= i2 && i2 < |xs|
; [eval] 0 <= i2
; [eval] 0 <= i2 ==> i2 < |xs|
; [eval] 0 <= i2
(push) ; 6
(push) ; 7
(assert (not (not (<= 0 i2@25))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= 0 i2@25)))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 17] 0 <= i2@25
(assert (<= 0 i2@25))
; [eval] i2 < |xs|
; [eval] |xs|
(pop) ; 7
(push) ; 7
; [else-branch 17] !0 <= i2@25
(assert (not (<= 0 i2@25)))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(push) ; 6
(push) ; 7
(assert (not (not (and (<= 0 i2@25) (implies (<= 0 i2@25) (< i2@25 ($Seq.length xs@15)))))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (and (<= 0 i2@25) (implies (<= 0 i2@25) (< i2@25 ($Seq.length xs@15))))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 18] 0 <= i2@25 && 0 <= i2@25 ==> i2@25 < |xs@15|
(assert (and (<= 0 i2@25) (implies (<= 0 i2@25) (< i2@25 ($Seq.length xs@15)))))
; [eval] xs[i2] != key
; [eval] xs[i2]
(pop) ; 7
(push) ; 7
; [else-branch 18] !0 <= i2@25 && 0 <= i2@25 ==> i2@25 < |xs@15|
(assert (not (and (<= 0 i2@25) (implies (<= 0 i2@25) (< i2@25 ($Seq.length xs@15))))))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(pop) ; 5
; Nested auxiliary terms
(pop) ; 4
(push) ; 4
; [else-branch 16] -1 != res@17
(assert (not (= (- 0 1) res@17)))
(pop) ; 4
(pop) ; 3
; Joined path conditions
; Joined path conditions
(assert (implies
  (= (- 0 1) res@17)
  (forall ((i2@25 Int)) (!
    (implies
      (and (<= 0 i2@25) (implies (<= 0 i2@25) (< i2@25 ($Seq.length xs@15))))
      (not (= ($Seq.index xs@15 i2@25) key@16)))
    :pattern (($Seq.index xs@15 i2@25))
    :qid |prog.l33|))))
(pop) ; 2
(push) ; 2
; [exec]
; low := 0
; [exec]
; high := |xs|
; [eval] |xs|
; [exec]
; index := -1
; [eval] -1
; loop at tmp2078.tmp@41.2
(declare-const mid@26 Int)
(declare-const low@27 Int)
(declare-const high@28 Int)
(declare-const index@29 Int)
(push) ; 3
; Loop: Check specs well-definedness
(declare-const $t@30 $Snap)
(assert (= $t@30 ($Snap.combine $Snap.unit $Snap.unit)))
; [eval] 0 <= low && (low <= high && high <= |xs|)
; [eval] 0 <= low
; [eval] 0 <= low ==> low <= high && high <= |xs|
; [eval] 0 <= low
(push) ; 4
(push) ; 5
(assert (not (not (<= 0 low@27))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 low@27)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 19] 0 <= low@27
(assert (<= 0 low@27))
; [eval] low <= high && high <= |xs|
; [eval] low <= high
; [eval] low <= high ==> high <= |xs|
; [eval] low <= high
(push) ; 6
(push) ; 7
(assert (not (not (<= low@27 high@28))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= low@27 high@28)))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 20] low@27 <= high@28
(assert (<= low@27 high@28))
; [eval] high <= |xs|
; [eval] |xs|
(pop) ; 7
(push) ; 7
; [else-branch 20] !low@27 <= high@28
(assert (not (<= low@27 high@28)))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(pop) ; 5
(push) ; 5
; [else-branch 19] !0 <= low@27
(assert (not (<= 0 low@27)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (and
  (<= 0 low@27)
  (implies
    (<= 0 low@27)
    (and
      (<= low@27 high@28)
      (implies (<= low@27 high@28) (<= high@28 ($Seq.length xs@15)))))))
; [eval] index == -1 ==> (forall i3: Int :: 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key)
; [eval] index == -1
; [eval] -1
(push) ; 4
(push) ; 5
(assert (not (not (= index@29 (- 0 1)))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (= index@29 (- 0 1))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 21] index@29 == -1
(assert (= index@29 (- 0 1)))
; [eval] (forall i3: Int :: 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key)
(declare-const i3@31 Int)
(push) ; 6
; [eval] 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key
; [eval] 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high))
; [eval] 0 <= i3
; [eval] 0 <= i3 ==> i3 < |xs| && !(low <= i3 && i3 < high)
; [eval] 0 <= i3
(push) ; 7
(push) ; 8
(assert (not (not (<= 0 i3@31))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
(assert (not (<= 0 i3@31)))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
; [then-branch 22] 0 <= i3@31
(assert (<= 0 i3@31))
; [eval] i3 < |xs| && !(low <= i3 && i3 < high)
; [eval] i3 < |xs|
; [eval] |xs|
; [eval] i3 < |xs| ==> !(low <= i3 && i3 < high)
; [eval] i3 < |xs|
; [eval] |xs|
(push) ; 9
(push) ; 10
(assert (not (not (< i3@31 ($Seq.length xs@15)))))
(check-sat)
; unknown
(pop) ; 10
; 0,00s
; (get-info :all-statistics)
(push) ; 10
(assert (not (< i3@31 ($Seq.length xs@15))))
(check-sat)
; unknown
(pop) ; 10
; 0,00s
; (get-info :all-statistics)
(push) ; 10
; [then-branch 23] i3@31 < |xs@15|
(assert (< i3@31 ($Seq.length xs@15)))
; [eval] !(low <= i3 && i3 < high)
; [eval] low <= i3 && i3 < high
; [eval] low <= i3
; [eval] low <= i3 ==> i3 < high
; [eval] low <= i3
(push) ; 11
(push) ; 12
(assert (not (not (<= low@27 i3@31))))
(check-sat)
; unknown
(pop) ; 12
; 0,00s
; (get-info :all-statistics)
(push) ; 12
(assert (not (<= low@27 i3@31)))
(check-sat)
; unknown
(pop) ; 12
; 0,00s
; (get-info :all-statistics)
(push) ; 12
; [then-branch 24] low@27 <= i3@31
(assert (<= low@27 i3@31))
; [eval] i3 < high
(pop) ; 12
(push) ; 12
; [else-branch 24] !low@27 <= i3@31
(assert (not (<= low@27 i3@31)))
(pop) ; 12
(pop) ; 11
; Joined path conditions
; Joined path conditions
(pop) ; 10
(push) ; 10
; [else-branch 23] !i3@31 < |xs@15|
(assert (not (< i3@31 ($Seq.length xs@15))))
(pop) ; 10
(pop) ; 9
; Joined path conditions
; Joined path conditions
(pop) ; 8
(push) ; 8
; [else-branch 22] !0 <= i3@31
(assert (not (<= 0 i3@31)))
(pop) ; 8
(pop) ; 7
; Joined path conditions
; Joined path conditions
(push) ; 7
(push) ; 8
(assert (not (not
  (and
    (<= 0 i3@31)
    (implies
      (<= 0 i3@31)
      (and
        (< i3@31 ($Seq.length xs@15))
        (implies
          (< i3@31 ($Seq.length xs@15))
          (not
            (and (<= low@27 i3@31) (implies (<= low@27 i3@31) (< i3@31 high@28)))))))))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
(assert (not (and
  (<= 0 i3@31)
  (implies
    (<= 0 i3@31)
    (and
      (< i3@31 ($Seq.length xs@15))
      (implies
        (< i3@31 ($Seq.length xs@15))
        (not
          (and (<= low@27 i3@31) (implies (<= low@27 i3@31) (< i3@31 high@28))))))))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
; [then-branch 25] 0 <= i3@31 && 0 <= i3@31 ==> i3@31 < |xs@15| && i3@31 < |xs@15| ==> !low@27 <= i3@31 && low@27 <= i3@31 ==> i3@31 < high@28
(assert (and
  (<= 0 i3@31)
  (implies
    (<= 0 i3@31)
    (and
      (< i3@31 ($Seq.length xs@15))
      (implies
        (< i3@31 ($Seq.length xs@15))
        (not
          (and (<= low@27 i3@31) (implies (<= low@27 i3@31) (< i3@31 high@28)))))))))
; [eval] xs[i3] != key
; [eval] xs[i3]
(pop) ; 8
(push) ; 8
; [else-branch 25] !0 <= i3@31 && 0 <= i3@31 ==> i3@31 < |xs@15| && i3@31 < |xs@15| ==> !low@27 <= i3@31 && low@27 <= i3@31 ==> i3@31 < high@28
(assert (not
  (and
    (<= 0 i3@31)
    (implies
      (<= 0 i3@31)
      (and
        (< i3@31 ($Seq.length xs@15))
        (implies
          (< i3@31 ($Seq.length xs@15))
          (not
            (and (<= low@27 i3@31) (implies (<= low@27 i3@31) (< i3@31 high@28))))))))))
(pop) ; 8
(pop) ; 7
; Joined path conditions
; Joined path conditions
(pop) ; 6
; Nested auxiliary terms
(pop) ; 5
(push) ; 5
; [else-branch 21] index@29 != -1
(assert (not (= index@29 (- 0 1))))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (implies
  (= index@29 (- 0 1))
  (forall ((i3@31 Int)) (!
    (implies
      (and
        (<= 0 i3@31)
        (implies
          (<= 0 i3@31)
          (and
            (< i3@31 ($Seq.length xs@15))
            (implies
              (< i3@31 ($Seq.length xs@15))
              (not
                (and
                  (<= low@27 i3@31)
                  (implies (<= low@27 i3@31) (< i3@31 high@28))))))))
      (not (= ($Seq.index xs@15 i3@31) key@16)))
    :pattern (($Seq.index xs@15 i3@31))
    :qid |prog.l43|))))
; [eval] -1 <= index && index < |xs|
; [eval] -1 <= index
; [eval] -1
; [eval] -1 <= index ==> index < |xs|
; [eval] -1 <= index
; [eval] -1
(push) ; 4
(push) ; 5
(assert (not (not (<= (- 0 1) index@29))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= (- 0 1) index@29)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 26] -1 <= index@29
(assert (<= (- 0 1) index@29))
; [eval] index < |xs|
; [eval] |xs|
(pop) ; 5
(push) ; 5
; [else-branch 26] !-1 <= index@29
(assert (not (<= (- 0 1) index@29)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (and
  (<= (- 0 1) index@29)
  (implies (<= (- 0 1) index@29) (< index@29 ($Seq.length xs@15)))))
; [eval] 0 <= index ==> xs[index] == key
; [eval] 0 <= index
(push) ; 4
(push) ; 5
(assert (not (not (<= 0 index@29))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 index@29)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 27] 0 <= index@29
(assert (<= 0 index@29))
; [eval] xs[index] == key
; [eval] xs[index]
(pop) ; 5
(push) ; 5
; [else-branch 27] !0 <= index@29
(assert (not (<= 0 index@29)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (implies (<= 0 index@29) (= ($Seq.index xs@15 index@29) key@16)))
(declare-const $t@32 $Snap)
(assert (= $t@32 $Snap.unit))
; [eval] low < high && index == -1
; [eval] low < high
; [eval] low < high ==> index == -1
; [eval] low < high
(push) ; 4
(push) ; 5
(assert (not (not (< low@27 high@28))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (< low@27 high@28)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 28] low@27 < high@28
(assert (< low@27 high@28))
; [eval] index == -1
; [eval] -1
(pop) ; 5
(push) ; 5
; [else-branch 28] !low@27 < high@28
(assert (not (< low@27 high@28)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (and (< low@27 high@28) (implies (< low@27 high@28) (= index@29 (- 0 1)))))
(check-sat)
; unknown
(pop) ; 3
(push) ; 3
; Loop: Establish loop invariant
; [eval] 0 <= low && (low <= high && high <= |xs|)
; [eval] 0 <= low
; [eval] 0 <= low ==> low <= high && high <= |xs|
; [eval] 0 <= low
(push) ; 4
(push) ; 5
(assert (not false))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 29] True
; [eval] low <= high && high <= |xs|
; [eval] low <= high
; [eval] low <= high ==> high <= |xs|
; [eval] low <= high
(push) ; 6
(push) ; 7
(assert (not (not (<= 0 ($Seq.length xs@15)))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= 0 ($Seq.length xs@15))))
(check-sat)
; unsat
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 30] 0 <= |xs@15|
(assert (<= 0 ($Seq.length xs@15)))
; [eval] high <= |xs|
; [eval] |xs|
(pop) ; 7
; [dead else-branch 30] !0 <= |xs@15|
(pop) ; 6
; Joined path conditions
(pop) ; 5
; [dead else-branch 29] False
(pop) ; 4
; Joined path conditions
(set-option :timeout 0)
(push) ; 4
(assert (not (<= 0 ($Seq.length xs@15))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (<= 0 ($Seq.length xs@15)))
; [eval] index == -1 ==> (forall i3: Int :: 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key)
; [eval] index == -1
; [eval] -1
(push) ; 4
(set-option :timeout 250)
(push) ; 5
(assert (not false))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 31] True
; [eval] (forall i3: Int :: 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key)
(declare-const i3@33 Int)
(push) ; 6
; [eval] 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key
; [eval] 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high))
; [eval] 0 <= i3
; [eval] 0 <= i3 ==> i3 < |xs| && !(low <= i3 && i3 < high)
; [eval] 0 <= i3
(push) ; 7
(push) ; 8
(assert (not (not (<= 0 i3@33))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
(assert (not (<= 0 i3@33)))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
; [then-branch 32] 0 <= i3@33
(assert (<= 0 i3@33))
; [eval] i3 < |xs| && !(low <= i3 && i3 < high)
; [eval] i3 < |xs|
; [eval] |xs|
; [eval] i3 < |xs| ==> !(low <= i3 && i3 < high)
; [eval] i3 < |xs|
; [eval] |xs|
(push) ; 9
(push) ; 10
(assert (not (not (< i3@33 ($Seq.length xs@15)))))
(check-sat)
; unknown
(pop) ; 10
; 0,00s
; (get-info :all-statistics)
(push) ; 10
(assert (not (< i3@33 ($Seq.length xs@15))))
(check-sat)
; unknown
(pop) ; 10
; 0,00s
; (get-info :all-statistics)
(push) ; 10
; [then-branch 33] i3@33 < |xs@15|
(assert (< i3@33 ($Seq.length xs@15)))
; [eval] !(low <= i3 && i3 < high)
; [eval] low <= i3 && i3 < high
; [eval] low <= i3
; [eval] low <= i3 ==> i3 < high
; [eval] low <= i3
(push) ; 11
(push) ; 12
(assert (not (not (<= 0 i3@33))))
(check-sat)
; unknown
(pop) ; 12
; 0,00s
; (get-info :all-statistics)
(push) ; 12
; [then-branch 34] 0 <= i3@33
; [eval] i3 < high
(pop) ; 12
; [dead else-branch 34] !0 <= i3@33
(pop) ; 11
; Joined path conditions
(pop) ; 10
(push) ; 10
; [else-branch 33] !i3@33 < |xs@15|
(assert (not (< i3@33 ($Seq.length xs@15))))
(pop) ; 10
(pop) ; 9
; Joined path conditions
; Joined path conditions
(pop) ; 8
(push) ; 8
; [else-branch 32] !0 <= i3@33
(assert (not (<= 0 i3@33)))
(pop) ; 8
(pop) ; 7
; Joined path conditions
; Joined path conditions
(push) ; 7
(push) ; 8
(assert (not (not
  (and
    (<= 0 i3@33)
    (implies
      (<= 0 i3@33)
      (and
        (< i3@33 ($Seq.length xs@15))
        (implies
          (< i3@33 ($Seq.length xs@15))
          (not
            (and
              (<= 0 i3@33)
              (implies (<= 0 i3@33) (< i3@33 ($Seq.length xs@15))))))))))))
(check-sat)
; unsat
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
; [dead then-branch 35] 0 <= i3@33 && 0 <= i3@33 ==> i3@33 < |xs@15| && i3@33 < |xs@15| ==> !0 <= i3@33 && 0 <= i3@33 ==> i3@33 < |xs@15|
(push) ; 8
; [else-branch 35] !0 <= i3@33 && 0 <= i3@33 ==> i3@33 < |xs@15| && i3@33 < |xs@15| ==> !0 <= i3@33 && 0 <= i3@33 ==> i3@33 < |xs@15|
(assert (not
  (and
    (<= 0 i3@33)
    (implies
      (<= 0 i3@33)
      (and
        (< i3@33 ($Seq.length xs@15))
        (implies
          (< i3@33 ($Seq.length xs@15))
          (not
            (and
              (<= 0 i3@33)
              (implies (<= 0 i3@33) (< i3@33 ($Seq.length xs@15)))))))))))
(pop) ; 8
(pop) ; 7
; Joined path conditions
; [eval] xs[i3]
(pop) ; 6
; Nested auxiliary terms
(pop) ; 5
; [dead else-branch 31] False
(pop) ; 4
; Joined path conditions
; [eval] -1 <= index && index < |xs|
; [eval] -1 <= index
; [eval] -1
; [eval] -1 <= index ==> index < |xs|
; [eval] -1 <= index
; [eval] -1
(push) ; 4
(push) ; 5
(assert (not false))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 36] True
; [eval] index < |xs|
; [eval] |xs|
(pop) ; 5
; [dead else-branch 36] False
(pop) ; 4
; Joined path conditions
(set-option :timeout 0)
(push) ; 4
(assert (not (< (- 0 1) ($Seq.length xs@15))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (< (- 0 1) ($Seq.length xs@15)))
; [eval] 0 <= index ==> xs[index] == key
; [eval] 0 <= index
(push) ; 4
; [dead then-branch 37] False
(push) ; 5
; [else-branch 37] True
(pop) ; 5
(pop) ; 4
; Joined path conditions
(pop) ; 3
; Loop: Verify loop body
(push) ; 3
(assert (and (< low@27 high@28) (implies (< low@27 high@28) (= index@29 (- 0 1)))))
(assert (= $t@32 $Snap.unit))
(assert (implies (<= 0 index@29) (= ($Seq.index xs@15 index@29) key@16)))
(assert (and
  (<= (- 0 1) index@29)
  (implies (<= (- 0 1) index@29) (< index@29 ($Seq.length xs@15)))))
(assert (implies
  (= index@29 (- 0 1))
  (forall ((i3@31 Int)) (!
    (implies
      (and
        (<= 0 i3@31)
        (implies
          (<= 0 i3@31)
          (and
            (< i3@31 ($Seq.length xs@15))
            (implies
              (< i3@31 ($Seq.length xs@15))
              (not
                (and
                  (<= low@27 i3@31)
                  (implies (<= low@27 i3@31) (< i3@31 high@28))))))))
      (not (= ($Seq.index xs@15 i3@31) key@16)))
    :pattern (($Seq.index xs@15 i3@31))
    :qid |prog.l43|))))
(assert (and
  (<= 0 low@27)
  (implies
    (<= 0 low@27)
    (and
      (<= low@27 high@28)
      (implies (<= low@27 high@28) (<= high@28 ($Seq.length xs@15)))))))
(assert (= $t@30 ($Snap.combine $Snap.unit $Snap.unit)))
; [exec]
; mid := (low + high) \ 2
; [eval] (low + high) \ 2
; [eval] low + high
(push) ; 4
(assert (not (not (= 2 0))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(declare-const mid@34 Int)
(assert (= mid@34 (div (+ low@27 high@28) 2)))
; [eval] xs[mid] < key
; [eval] xs[mid]
(set-option :timeout 250)
(push) ; 4
(assert (not (not (< ($Seq.index xs@15 mid@34) key@16))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (< ($Seq.index xs@15 mid@34) key@16)))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
; [then-branch 38] xs@15[mid@34] < key@16
(assert (< ($Seq.index xs@15 mid@34) key@16))
; [exec]
; low := mid + 1
; [eval] mid + 1
(declare-const low@35 Int)
(assert (= low@35 (+ mid@34 1)))
; [eval] 0 <= low && (low <= high && high <= |xs|)
; [eval] 0 <= low
; [eval] 0 <= low ==> low <= high && high <= |xs|
; [eval] 0 <= low
(push) ; 5
(push) ; 6
(assert (not (not (<= 0 low@35))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (<= 0 low@35)))
(check-sat)
; unsat
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 39] 0 <= low@35
(assert (<= 0 low@35))
; [eval] low <= high && high <= |xs|
; [eval] low <= high
; [eval] low <= high ==> high <= |xs|
; [eval] low <= high
(push) ; 7
(push) ; 8
(assert (not (not (<= low@35 high@28))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
(assert (not (<= low@35 high@28)))
(check-sat)
; unsat
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
; [then-branch 40] low@35 <= high@28
(assert (<= low@35 high@28))
; [eval] high <= |xs|
; [eval] |xs|
(pop) ; 8
; [dead else-branch 40] !low@35 <= high@28
(pop) ; 7
; Joined path conditions
(pop) ; 6
; [dead else-branch 39] !0 <= low@35
(pop) ; 5
; Joined path conditions
(set-option :timeout 0)
(push) ; 5
(assert (not (and
  (<= 0 low@35)
  (implies
    (<= 0 low@35)
    (and
      (<= low@35 high@28)
      (implies (<= low@35 high@28) (<= high@28 ($Seq.length xs@15))))))))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(assert (and
  (<= 0 low@35)
  (implies
    (<= 0 low@35)
    (and
      (<= low@35 high@28)
      (implies (<= low@35 high@28) (<= high@28 ($Seq.length xs@15)))))))
; [eval] index == -1 ==> (forall i3: Int :: 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key)
; [eval] index == -1
; [eval] -1
(push) ; 5
(set-option :timeout 250)
(push) ; 6
(assert (not (not (= index@29 (- 0 1)))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (= index@29 (- 0 1))))
(check-sat)
; unsat
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 41] index@29 == -1
(assert (= index@29 (- 0 1)))
; [eval] (forall i3: Int :: 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key)
(declare-const i3@36 Int)
(push) ; 7
; [eval] 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key
; [eval] 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high))
; [eval] 0 <= i3
; [eval] 0 <= i3 ==> i3 < |xs| && !(low <= i3 && i3 < high)
; [eval] 0 <= i3
(push) ; 8
(push) ; 9
(assert (not (not (<= 0 i3@36))))
(check-sat)
; unknown
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
(assert (not (<= 0 i3@36)))
(check-sat)
; unknown
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
; [then-branch 42] 0 <= i3@36
(assert (<= 0 i3@36))
; [eval] i3 < |xs| && !(low <= i3 && i3 < high)
; [eval] i3 < |xs|
; [eval] |xs|
; [eval] i3 < |xs| ==> !(low <= i3 && i3 < high)
; [eval] i3 < |xs|
; [eval] |xs|
(push) ; 10
(push) ; 11
(assert (not (not (< i3@36 ($Seq.length xs@15)))))
(check-sat)
; unknown
(pop) ; 11
; 0,00s
; (get-info :all-statistics)
(push) ; 11
(assert (not (< i3@36 ($Seq.length xs@15))))
(check-sat)
; unknown
(pop) ; 11
; 0,00s
; (get-info :all-statistics)
(push) ; 11
; [then-branch 43] i3@36 < |xs@15|
(assert (< i3@36 ($Seq.length xs@15)))
; [eval] !(low <= i3 && i3 < high)
; [eval] low <= i3 && i3 < high
; [eval] low <= i3
; [eval] low <= i3 ==> i3 < high
; [eval] low <= i3
(push) ; 12
(push) ; 13
(assert (not (not (<= low@35 i3@36))))
(check-sat)
; unknown
(pop) ; 13
; 0,00s
; (get-info :all-statistics)
(push) ; 13
(assert (not (<= low@35 i3@36)))
(check-sat)
; unknown
(pop) ; 13
; 0,00s
; (get-info :all-statistics)
(push) ; 13
; [then-branch 44] low@35 <= i3@36
(assert (<= low@35 i3@36))
; [eval] i3 < high
(pop) ; 13
(push) ; 13
; [else-branch 44] !low@35 <= i3@36
(assert (not (<= low@35 i3@36)))
(pop) ; 13
(pop) ; 12
; Joined path conditions
; Joined path conditions
(pop) ; 11
(push) ; 11
; [else-branch 43] !i3@36 < |xs@15|
(assert (not (< i3@36 ($Seq.length xs@15))))
(pop) ; 11
(pop) ; 10
; Joined path conditions
; Joined path conditions
(pop) ; 9
(push) ; 9
; [else-branch 42] !0 <= i3@36
(assert (not (<= 0 i3@36)))
(pop) ; 9
(pop) ; 8
; Joined path conditions
; Joined path conditions
(push) ; 8
(push) ; 9
(assert (not (not
  (and
    (<= 0 i3@36)
    (implies
      (<= 0 i3@36)
      (and
        (< i3@36 ($Seq.length xs@15))
        (implies
          (< i3@36 ($Seq.length xs@15))
          (not
            (and (<= low@35 i3@36) (implies (<= low@35 i3@36) (< i3@36 high@28)))))))))))
(check-sat)
; unknown
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
(assert (not (and
  (<= 0 i3@36)
  (implies
    (<= 0 i3@36)
    (and
      (< i3@36 ($Seq.length xs@15))
      (implies
        (< i3@36 ($Seq.length xs@15))
        (not
          (and (<= low@35 i3@36) (implies (<= low@35 i3@36) (< i3@36 high@28))))))))))
(check-sat)
; unknown
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
; [then-branch 45] 0 <= i3@36 && 0 <= i3@36 ==> i3@36 < |xs@15| && i3@36 < |xs@15| ==> !low@35 <= i3@36 && low@35 <= i3@36 ==> i3@36 < high@28
(assert (and
  (<= 0 i3@36)
  (implies
    (<= 0 i3@36)
    (and
      (< i3@36 ($Seq.length xs@15))
      (implies
        (< i3@36 ($Seq.length xs@15))
        (not
          (and (<= low@35 i3@36) (implies (<= low@35 i3@36) (< i3@36 high@28)))))))))
; [eval] xs[i3] != key
; [eval] xs[i3]
(pop) ; 9
(push) ; 9
; [else-branch 45] !0 <= i3@36 && 0 <= i3@36 ==> i3@36 < |xs@15| && i3@36 < |xs@15| ==> !low@35 <= i3@36 && low@35 <= i3@36 ==> i3@36 < high@28
(assert (not
  (and
    (<= 0 i3@36)
    (implies
      (<= 0 i3@36)
      (and
        (< i3@36 ($Seq.length xs@15))
        (implies
          (< i3@36 ($Seq.length xs@15))
          (not
            (and (<= low@35 i3@36) (implies (<= low@35 i3@36) (< i3@36 high@28))))))))))
(pop) ; 9
(pop) ; 8
; Joined path conditions
; Joined path conditions
(pop) ; 7
; Nested auxiliary terms
(pop) ; 6
; [dead else-branch 41] index@29 != -1
(pop) ; 5
; Joined path conditions
(set-option :timeout 0)
(push) ; 5
(assert (not (implies
  (= index@29 (- 0 1))
  (forall ((i3@36 Int)) (!
    (implies
      (and
        (<= 0 i3@36)
        (implies
          (<= 0 i3@36)
          (and
            (< i3@36 ($Seq.length xs@15))
            (implies
              (< i3@36 ($Seq.length xs@15))
              (not
                (and
                  (<= low@35 i3@36)
                  (implies (<= low@35 i3@36) (< i3@36 high@28))))))))
      (not (= ($Seq.index xs@15 i3@36) key@16)))
    :pattern (($Seq.index xs@15 i3@36))
    :qid |prog.l43|)))))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(assert (implies
  (= index@29 (- 0 1))
  (forall ((i3@36 Int)) (!
    (implies
      (and
        (<= 0 i3@36)
        (implies
          (<= 0 i3@36)
          (and
            (< i3@36 ($Seq.length xs@15))
            (implies
              (< i3@36 ($Seq.length xs@15))
              (not
                (and
                  (<= low@35 i3@36)
                  (implies (<= low@35 i3@36) (< i3@36 high@28))))))))
      (not (= ($Seq.index xs@15 i3@36) key@16)))
    :pattern (($Seq.index xs@15 i3@36))
    :qid |prog.l43|))))
; [eval] -1 <= index && index < |xs|
; [eval] -1 <= index
; [eval] -1
; [eval] -1 <= index ==> index < |xs|
; [eval] -1 <= index
; [eval] -1
(push) ; 5
(set-option :timeout 250)
(push) ; 6
(assert (not (not (<= (- 0 1) index@29))))
(check-sat)
; unknown
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
(assert (not (<= (- 0 1) index@29)))
(check-sat)
; unsat
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(push) ; 6
; [then-branch 46] -1 <= index@29
(assert (<= (- 0 1) index@29))
; [eval] index < |xs|
; [eval] |xs|
(pop) ; 6
; [dead else-branch 46] !-1 <= index@29
(pop) ; 5
; Joined path conditions
; [eval] 0 <= index ==> xs[index] == key
; [eval] 0 <= index
(push) ; 5
(push) ; 6
(assert (not (not (<= 0 index@29))))
(check-sat)
; unsat
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
; [dead then-branch 47] 0 <= index@29
(push) ; 6
; [else-branch 47] !0 <= index@29
(assert (not (<= 0 index@29)))
(pop) ; 6
(pop) ; 5
; Joined path conditions
(pop) ; 4
(push) ; 4
; [else-branch 38] !xs@15[mid@34] < key@16
(assert (not (< ($Seq.index xs@15 mid@34) key@16)))
(pop) ; 4
; [eval] !(xs[mid] < key)
; [eval] xs[mid] < key
; [eval] xs[mid]
(push) ; 4
(assert (not (< ($Seq.index xs@15 mid@34) key@16)))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
(assert (not (not (< ($Seq.index xs@15 mid@34) key@16))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(push) ; 4
; [then-branch 48] !xs@15[mid@34] < key@16
(assert (not (< ($Seq.index xs@15 mid@34) key@16)))
; [eval] key < xs[mid]
; [eval] xs[mid]
(push) ; 5
(assert (not (not (< key@16 ($Seq.index xs@15 mid@34)))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (< key@16 ($Seq.index xs@15 mid@34))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 49] key@16 < xs@15[mid@34]
(assert (< key@16 ($Seq.index xs@15 mid@34)))
; [exec]
; high := mid
; [eval] 0 <= low && (low <= high && high <= |xs|)
; [eval] 0 <= low
; [eval] 0 <= low ==> low <= high && high <= |xs|
; [eval] 0 <= low
(push) ; 6
(push) ; 7
(assert (not (not (<= 0 low@27))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= 0 low@27)))
(check-sat)
; unsat
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 50] 0 <= low@27
(assert (<= 0 low@27))
; [eval] low <= high && high <= |xs|
; [eval] low <= high
; [eval] low <= high ==> high <= |xs|
; [eval] low <= high
(push) ; 8
(push) ; 9
(assert (not (not (<= low@27 mid@34))))
(check-sat)
; unknown
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
(assert (not (<= low@27 mid@34)))
(check-sat)
; unsat
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
; [then-branch 51] low@27 <= mid@34
(assert (<= low@27 mid@34))
; [eval] high <= |xs|
; [eval] |xs|
(pop) ; 9
; [dead else-branch 51] !low@27 <= mid@34
(pop) ; 8
; Joined path conditions
(pop) ; 7
; [dead else-branch 50] !0 <= low@27
(pop) ; 6
; Joined path conditions
(set-option :timeout 0)
(push) ; 6
(assert (not (and
  (<= 0 low@27)
  (implies
    (<= 0 low@27)
    (and
      (<= low@27 mid@34)
      (implies (<= low@27 mid@34) (<= mid@34 ($Seq.length xs@15))))))))
(check-sat)
; unsat
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(assert (and
  (<= 0 low@27)
  (implies
    (<= 0 low@27)
    (and
      (<= low@27 mid@34)
      (implies (<= low@27 mid@34) (<= mid@34 ($Seq.length xs@15)))))))
; [eval] index == -1 ==> (forall i3: Int :: 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key)
; [eval] index == -1
; [eval] -1
(push) ; 6
(set-option :timeout 250)
(push) ; 7
(assert (not (not (= index@29 (- 0 1)))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (= index@29 (- 0 1))))
(check-sat)
; unsat
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 52] index@29 == -1
(assert (= index@29 (- 0 1)))
; [eval] (forall i3: Int :: 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key)
(declare-const i3@37 Int)
(push) ; 8
; [eval] 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key
; [eval] 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high))
; [eval] 0 <= i3
; [eval] 0 <= i3 ==> i3 < |xs| && !(low <= i3 && i3 < high)
; [eval] 0 <= i3
(push) ; 9
(push) ; 10
(assert (not (not (<= 0 i3@37))))
(check-sat)
; unknown
(pop) ; 10
; 0,00s
; (get-info :all-statistics)
(push) ; 10
(assert (not (<= 0 i3@37)))
(check-sat)
; unknown
(pop) ; 10
; 0,00s
; (get-info :all-statistics)
(push) ; 10
; [then-branch 53] 0 <= i3@37
(assert (<= 0 i3@37))
; [eval] i3 < |xs| && !(low <= i3 && i3 < high)
; [eval] i3 < |xs|
; [eval] |xs|
; [eval] i3 < |xs| ==> !(low <= i3 && i3 < high)
; [eval] i3 < |xs|
; [eval] |xs|
(push) ; 11
(push) ; 12
(assert (not (not (< i3@37 ($Seq.length xs@15)))))
(check-sat)
; unknown
(pop) ; 12
; 0,00s
; (get-info :all-statistics)
(push) ; 12
(assert (not (< i3@37 ($Seq.length xs@15))))
(check-sat)
; unknown
(pop) ; 12
; 0,00s
; (get-info :all-statistics)
(push) ; 12
; [then-branch 54] i3@37 < |xs@15|
(assert (< i3@37 ($Seq.length xs@15)))
; [eval] !(low <= i3 && i3 < high)
; [eval] low <= i3 && i3 < high
; [eval] low <= i3
; [eval] low <= i3 ==> i3 < high
; [eval] low <= i3
(push) ; 13
(push) ; 14
(assert (not (not (<= low@27 i3@37))))
(check-sat)
; unknown
(pop) ; 14
; 0,00s
; (get-info :all-statistics)
(push) ; 14
(assert (not (<= low@27 i3@37)))
(check-sat)
; unknown
(pop) ; 14
; 0,00s
; (get-info :all-statistics)
(push) ; 14
; [then-branch 55] low@27 <= i3@37
(assert (<= low@27 i3@37))
; [eval] i3 < high
(pop) ; 14
(push) ; 14
; [else-branch 55] !low@27 <= i3@37
(assert (not (<= low@27 i3@37)))
(pop) ; 14
(pop) ; 13
; Joined path conditions
; Joined path conditions
(pop) ; 12
(push) ; 12
; [else-branch 54] !i3@37 < |xs@15|
(assert (not (< i3@37 ($Seq.length xs@15))))
(pop) ; 12
(pop) ; 11
; Joined path conditions
; Joined path conditions
(pop) ; 10
(push) ; 10
; [else-branch 53] !0 <= i3@37
(assert (not (<= 0 i3@37)))
(pop) ; 10
(pop) ; 9
; Joined path conditions
; Joined path conditions
(push) ; 9
(push) ; 10
(assert (not (not
  (and
    (<= 0 i3@37)
    (implies
      (<= 0 i3@37)
      (and
        (< i3@37 ($Seq.length xs@15))
        (implies
          (< i3@37 ($Seq.length xs@15))
          (not
            (and (<= low@27 i3@37) (implies (<= low@27 i3@37) (< i3@37 mid@34)))))))))))
(check-sat)
; unknown
(pop) ; 10
; 0,00s
; (get-info :all-statistics)
(push) ; 10
(assert (not (and
  (<= 0 i3@37)
  (implies
    (<= 0 i3@37)
    (and
      (< i3@37 ($Seq.length xs@15))
      (implies
        (< i3@37 ($Seq.length xs@15))
        (not
          (and (<= low@27 i3@37) (implies (<= low@27 i3@37) (< i3@37 mid@34))))))))))
(check-sat)
; unknown
(pop) ; 10
; 0,00s
; (get-info :all-statistics)
(push) ; 10
; [then-branch 56] 0 <= i3@37 && 0 <= i3@37 ==> i3@37 < |xs@15| && i3@37 < |xs@15| ==> !low@27 <= i3@37 && low@27 <= i3@37 ==> i3@37 < mid@34
(assert (and
  (<= 0 i3@37)
  (implies
    (<= 0 i3@37)
    (and
      (< i3@37 ($Seq.length xs@15))
      (implies
        (< i3@37 ($Seq.length xs@15))
        (not
          (and (<= low@27 i3@37) (implies (<= low@27 i3@37) (< i3@37 mid@34)))))))))
; [eval] xs[i3] != key
; [eval] xs[i3]
(pop) ; 10
(push) ; 10
; [else-branch 56] !0 <= i3@37 && 0 <= i3@37 ==> i3@37 < |xs@15| && i3@37 < |xs@15| ==> !low@27 <= i3@37 && low@27 <= i3@37 ==> i3@37 < mid@34
(assert (not
  (and
    (<= 0 i3@37)
    (implies
      (<= 0 i3@37)
      (and
        (< i3@37 ($Seq.length xs@15))
        (implies
          (< i3@37 ($Seq.length xs@15))
          (not
            (and (<= low@27 i3@37) (implies (<= low@27 i3@37) (< i3@37 mid@34))))))))))
(pop) ; 10
(pop) ; 9
; Joined path conditions
; Joined path conditions
(pop) ; 8
; Nested auxiliary terms
(pop) ; 7
; [dead else-branch 52] index@29 != -1
(pop) ; 6
; Joined path conditions
(set-option :timeout 0)
(push) ; 6
(assert (not (implies
  (= index@29 (- 0 1))
  (forall ((i3@37 Int)) (!
    (implies
      (and
        (<= 0 i3@37)
        (implies
          (<= 0 i3@37)
          (and
            (< i3@37 ($Seq.length xs@15))
            (implies
              (< i3@37 ($Seq.length xs@15))
              (not
                (and
                  (<= low@27 i3@37)
                  (implies (<= low@27 i3@37) (< i3@37 mid@34))))))))
      (not (= ($Seq.index xs@15 i3@37) key@16)))
    :pattern (($Seq.index xs@15 i3@37))
    :qid |prog.l43|)))))
(check-sat)
; unsat
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(assert (implies
  (= index@29 (- 0 1))
  (forall ((i3@37 Int)) (!
    (implies
      (and
        (<= 0 i3@37)
        (implies
          (<= 0 i3@37)
          (and
            (< i3@37 ($Seq.length xs@15))
            (implies
              (< i3@37 ($Seq.length xs@15))
              (not
                (and
                  (<= low@27 i3@37)
                  (implies (<= low@27 i3@37) (< i3@37 mid@34))))))))
      (not (= ($Seq.index xs@15 i3@37) key@16)))
    :pattern (($Seq.index xs@15 i3@37))
    :qid |prog.l43|))))
; [eval] -1 <= index && index < |xs|
; [eval] -1 <= index
; [eval] -1
; [eval] -1 <= index ==> index < |xs|
; [eval] -1 <= index
; [eval] -1
(push) ; 6
(set-option :timeout 250)
(push) ; 7
(assert (not (not (<= (- 0 1) index@29))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= (- 0 1) index@29)))
(check-sat)
; unsat
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 57] -1 <= index@29
(assert (<= (- 0 1) index@29))
; [eval] index < |xs|
; [eval] |xs|
(pop) ; 7
; [dead else-branch 57] !-1 <= index@29
(pop) ; 6
; Joined path conditions
; [eval] 0 <= index ==> xs[index] == key
; [eval] 0 <= index
(push) ; 6
(push) ; 7
(assert (not (not (<= 0 index@29))))
(check-sat)
; unsat
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
; [dead then-branch 58] 0 <= index@29
(push) ; 7
; [else-branch 58] !0 <= index@29
(assert (not (<= 0 index@29)))
(pop) ; 7
(pop) ; 6
; Joined path conditions
(pop) ; 5
(push) ; 5
; [else-branch 49] !key@16 < xs@15[mid@34]
(assert (not (< key@16 ($Seq.index xs@15 mid@34))))
(pop) ; 5
; [eval] !(key < xs[mid])
; [eval] key < xs[mid]
; [eval] xs[mid]
(push) ; 5
(assert (not (< key@16 ($Seq.index xs@15 mid@34))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (not (< key@16 ($Seq.index xs@15 mid@34)))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 59] !key@16 < xs@15[mid@34]
(assert (not (< key@16 ($Seq.index xs@15 mid@34))))
; [exec]
; index := mid
; [exec]
; high := mid
; [eval] 0 <= low && (low <= high && high <= |xs|)
; [eval] 0 <= low
; [eval] 0 <= low ==> low <= high && high <= |xs|
; [eval] 0 <= low
(push) ; 6
(push) ; 7
(assert (not (not (<= 0 low@27))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= 0 low@27)))
(check-sat)
; unsat
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 60] 0 <= low@27
(assert (<= 0 low@27))
; [eval] low <= high && high <= |xs|
; [eval] low <= high
; [eval] low <= high ==> high <= |xs|
; [eval] low <= high
(push) ; 8
(push) ; 9
(assert (not (not (<= low@27 mid@34))))
(check-sat)
; unknown
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
(assert (not (<= low@27 mid@34)))
(check-sat)
; unsat
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
; [then-branch 61] low@27 <= mid@34
(assert (<= low@27 mid@34))
; [eval] high <= |xs|
; [eval] |xs|
(pop) ; 9
; [dead else-branch 61] !low@27 <= mid@34
(pop) ; 8
; Joined path conditions
(pop) ; 7
; [dead else-branch 60] !0 <= low@27
(pop) ; 6
; Joined path conditions
(set-option :timeout 0)
(push) ; 6
(assert (not (and
  (<= 0 low@27)
  (implies
    (<= 0 low@27)
    (and
      (<= low@27 mid@34)
      (implies (<= low@27 mid@34) (<= mid@34 ($Seq.length xs@15))))))))
(check-sat)
; unsat
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(assert (and
  (<= 0 low@27)
  (implies
    (<= 0 low@27)
    (and
      (<= low@27 mid@34)
      (implies (<= low@27 mid@34) (<= mid@34 ($Seq.length xs@15)))))))
; [eval] index == -1 ==> (forall i3: Int :: 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key)
; [eval] index == -1
; [eval] -1
(push) ; 6
(set-option :timeout 250)
(push) ; 7
(assert (not (not (= mid@34 (- 0 1)))))
(check-sat)
; unsat
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
; [dead then-branch 62] mid@34 == -1
(push) ; 7
; [else-branch 62] mid@34 != -1
(assert (not (= mid@34 (- 0 1))))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; [eval] -1 <= index && index < |xs|
; [eval] -1 <= index
; [eval] -1
; [eval] -1 <= index ==> index < |xs|
; [eval] -1 <= index
; [eval] -1
(push) ; 6
(push) ; 7
(assert (not (not (<= (- 0 1) mid@34))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= (- 0 1) mid@34)))
(check-sat)
; unsat
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 63] -1 <= mid@34
(assert (<= (- 0 1) mid@34))
; [eval] index < |xs|
; [eval] |xs|
(pop) ; 7
; [dead else-branch 63] !-1 <= mid@34
(pop) ; 6
; Joined path conditions
(set-option :timeout 0)
(push) ; 6
(assert (not (and
  (<= (- 0 1) mid@34)
  (implies (<= (- 0 1) mid@34) (< mid@34 ($Seq.length xs@15))))))
(check-sat)
; unsat
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(assert (and
  (<= (- 0 1) mid@34)
  (implies (<= (- 0 1) mid@34) (< mid@34 ($Seq.length xs@15)))))
; [eval] 0 <= index ==> xs[index] == key
; [eval] 0 <= index
(push) ; 6
(set-option :timeout 250)
(push) ; 7
(assert (not (not (<= 0 mid@34))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= 0 mid@34)))
(check-sat)
; unsat
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 64] 0 <= mid@34
(assert (<= 0 mid@34))
; [eval] xs[index] == key
; [eval] xs[index]
(pop) ; 7
; [dead else-branch 64] !0 <= mid@34
(pop) ; 6
; Joined path conditions
(set-option :timeout 0)
(push) ; 6
(assert (not (implies (<= 0 mid@34) (= ($Seq.index xs@15 mid@34) key@16))))
(check-sat)
; unsat
(pop) ; 6
; 0,00s
; (get-info :all-statistics)
(assert (implies (<= 0 mid@34) (= ($Seq.index xs@15 mid@34) key@16)))
(pop) ; 5
(push) ; 5
; [else-branch 59] key@16 < xs@15[mid@34]
(assert (< key@16 ($Seq.index xs@15 mid@34)))
(pop) ; 5
(pop) ; 4
(push) ; 4
; [else-branch 48] xs@15[mid@34] < key@16
(assert (< ($Seq.index xs@15 mid@34) key@16))
(pop) ; 4
(pop) ; 3
; Loop: Continue after loop
(push) ; 3
(assert (< (- 0 1) ($Seq.length xs@15)))
(assert (<= 0 ($Seq.length xs@15)))
(declare-const $t@38 $Snap)
(assert (= $t@38 ($Snap.combine $Snap.unit $Snap.unit)))
; [eval] 0 <= low && (low <= high && high <= |xs|)
; [eval] 0 <= low
; [eval] 0 <= low ==> low <= high && high <= |xs|
; [eval] 0 <= low
(push) ; 4
(set-option :timeout 250)
(push) ; 5
(assert (not (not (<= 0 low@27))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 low@27)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 65] 0 <= low@27
(assert (<= 0 low@27))
; [eval] low <= high && high <= |xs|
; [eval] low <= high
; [eval] low <= high ==> high <= |xs|
; [eval] low <= high
(push) ; 6
(push) ; 7
(assert (not (not (<= low@27 high@28))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
(assert (not (<= low@27 high@28)))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 66] low@27 <= high@28
(assert (<= low@27 high@28))
; [eval] high <= |xs|
; [eval] |xs|
(pop) ; 7
(push) ; 7
; [else-branch 66] !low@27 <= high@28
(assert (not (<= low@27 high@28)))
(pop) ; 7
(pop) ; 6
; Joined path conditions
; Joined path conditions
(pop) ; 5
(push) ; 5
; [else-branch 65] !0 <= low@27
(assert (not (<= 0 low@27)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (and
  (<= 0 low@27)
  (implies
    (<= 0 low@27)
    (and
      (<= low@27 high@28)
      (implies (<= low@27 high@28) (<= high@28 ($Seq.length xs@15)))))))
; [eval] index == -1 ==> (forall i3: Int :: 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key)
; [eval] index == -1
; [eval] -1
(push) ; 4
(push) ; 5
(assert (not (not (= index@29 (- 0 1)))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (= index@29 (- 0 1))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 67] index@29 == -1
(assert (= index@29 (- 0 1)))
; [eval] (forall i3: Int :: 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key)
(declare-const i3@39 Int)
(push) ; 6
; [eval] 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high)) ==> xs[i3] != key
; [eval] 0 <= i3 && (i3 < |xs| && !(low <= i3 && i3 < high))
; [eval] 0 <= i3
; [eval] 0 <= i3 ==> i3 < |xs| && !(low <= i3 && i3 < high)
; [eval] 0 <= i3
(push) ; 7
(push) ; 8
(assert (not (not (<= 0 i3@39))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
(assert (not (<= 0 i3@39)))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
; [then-branch 68] 0 <= i3@39
(assert (<= 0 i3@39))
; [eval] i3 < |xs| && !(low <= i3 && i3 < high)
; [eval] i3 < |xs|
; [eval] |xs|
; [eval] i3 < |xs| ==> !(low <= i3 && i3 < high)
; [eval] i3 < |xs|
; [eval] |xs|
(push) ; 9
(push) ; 10
(assert (not (not (< i3@39 ($Seq.length xs@15)))))
(check-sat)
; unknown
(pop) ; 10
; 0,00s
; (get-info :all-statistics)
(push) ; 10
(assert (not (< i3@39 ($Seq.length xs@15))))
(check-sat)
; unknown
(pop) ; 10
; 0,00s
; (get-info :all-statistics)
(push) ; 10
; [then-branch 69] i3@39 < |xs@15|
(assert (< i3@39 ($Seq.length xs@15)))
; [eval] !(low <= i3 && i3 < high)
; [eval] low <= i3 && i3 < high
; [eval] low <= i3
; [eval] low <= i3 ==> i3 < high
; [eval] low <= i3
(push) ; 11
(push) ; 12
(assert (not (not (<= low@27 i3@39))))
(check-sat)
; unknown
(pop) ; 12
; 0,00s
; (get-info :all-statistics)
(push) ; 12
(assert (not (<= low@27 i3@39)))
(check-sat)
; unknown
(pop) ; 12
; 0,00s
; (get-info :all-statistics)
(push) ; 12
; [then-branch 70] low@27 <= i3@39
(assert (<= low@27 i3@39))
; [eval] i3 < high
(pop) ; 12
(push) ; 12
; [else-branch 70] !low@27 <= i3@39
(assert (not (<= low@27 i3@39)))
(pop) ; 12
(pop) ; 11
; Joined path conditions
; Joined path conditions
(pop) ; 10
(push) ; 10
; [else-branch 69] !i3@39 < |xs@15|
(assert (not (< i3@39 ($Seq.length xs@15))))
(pop) ; 10
(pop) ; 9
; Joined path conditions
; Joined path conditions
(pop) ; 8
(push) ; 8
; [else-branch 68] !0 <= i3@39
(assert (not (<= 0 i3@39)))
(pop) ; 8
(pop) ; 7
; Joined path conditions
; Joined path conditions
(push) ; 7
(push) ; 8
(assert (not (not
  (and
    (<= 0 i3@39)
    (implies
      (<= 0 i3@39)
      (and
        (< i3@39 ($Seq.length xs@15))
        (implies
          (< i3@39 ($Seq.length xs@15))
          (not
            (and (<= low@27 i3@39) (implies (<= low@27 i3@39) (< i3@39 high@28)))))))))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
(assert (not (and
  (<= 0 i3@39)
  (implies
    (<= 0 i3@39)
    (and
      (< i3@39 ($Seq.length xs@15))
      (implies
        (< i3@39 ($Seq.length xs@15))
        (not
          (and (<= low@27 i3@39) (implies (<= low@27 i3@39) (< i3@39 high@28))))))))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
; [then-branch 71] 0 <= i3@39 && 0 <= i3@39 ==> i3@39 < |xs@15| && i3@39 < |xs@15| ==> !low@27 <= i3@39 && low@27 <= i3@39 ==> i3@39 < high@28
(assert (and
  (<= 0 i3@39)
  (implies
    (<= 0 i3@39)
    (and
      (< i3@39 ($Seq.length xs@15))
      (implies
        (< i3@39 ($Seq.length xs@15))
        (not
          (and (<= low@27 i3@39) (implies (<= low@27 i3@39) (< i3@39 high@28)))))))))
; [eval] xs[i3] != key
; [eval] xs[i3]
(pop) ; 8
(push) ; 8
; [else-branch 71] !0 <= i3@39 && 0 <= i3@39 ==> i3@39 < |xs@15| && i3@39 < |xs@15| ==> !low@27 <= i3@39 && low@27 <= i3@39 ==> i3@39 < high@28
(assert (not
  (and
    (<= 0 i3@39)
    (implies
      (<= 0 i3@39)
      (and
        (< i3@39 ($Seq.length xs@15))
        (implies
          (< i3@39 ($Seq.length xs@15))
          (not
            (and (<= low@27 i3@39) (implies (<= low@27 i3@39) (< i3@39 high@28))))))))))
(pop) ; 8
(pop) ; 7
; Joined path conditions
; Joined path conditions
(pop) ; 6
; Nested auxiliary terms
(pop) ; 5
(push) ; 5
; [else-branch 67] index@29 != -1
(assert (not (= index@29 (- 0 1))))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (implies
  (= index@29 (- 0 1))
  (forall ((i3@39 Int)) (!
    (implies
      (and
        (<= 0 i3@39)
        (implies
          (<= 0 i3@39)
          (and
            (< i3@39 ($Seq.length xs@15))
            (implies
              (< i3@39 ($Seq.length xs@15))
              (not
                (and
                  (<= low@27 i3@39)
                  (implies (<= low@27 i3@39) (< i3@39 high@28))))))))
      (not (= ($Seq.index xs@15 i3@39) key@16)))
    :pattern (($Seq.index xs@15 i3@39))
    :qid |prog.l43|))))
; [eval] -1 <= index && index < |xs|
; [eval] -1 <= index
; [eval] -1
; [eval] -1 <= index ==> index < |xs|
; [eval] -1 <= index
; [eval] -1
(push) ; 4
(push) ; 5
(assert (not (not (<= (- 0 1) index@29))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= (- 0 1) index@29)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 72] -1 <= index@29
(assert (<= (- 0 1) index@29))
; [eval] index < |xs|
; [eval] |xs|
(pop) ; 5
(push) ; 5
; [else-branch 72] !-1 <= index@29
(assert (not (<= (- 0 1) index@29)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (and
  (<= (- 0 1) index@29)
  (implies (<= (- 0 1) index@29) (< index@29 ($Seq.length xs@15)))))
; [eval] 0 <= index ==> xs[index] == key
; [eval] 0 <= index
(push) ; 4
(push) ; 5
(assert (not (not (<= 0 index@29))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 index@29)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 73] 0 <= index@29
(assert (<= 0 index@29))
; [eval] xs[index] == key
; [eval] xs[index]
(pop) ; 5
(push) ; 5
; [else-branch 73] !0 <= index@29
(assert (not (<= 0 index@29)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (implies (<= 0 index@29) (= ($Seq.index xs@15 index@29) key@16)))
; [eval] !(low < high && index == -1)
; [eval] low < high && index == -1
; [eval] low < high
; [eval] low < high ==> index == -1
; [eval] low < high
(push) ; 4
(push) ; 5
(assert (not (not (< low@27 high@28))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (< low@27 high@28)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 74] low@27 < high@28
(assert (< low@27 high@28))
; [eval] index == -1
; [eval] -1
(pop) ; 5
(push) ; 5
; [else-branch 74] !low@27 < high@28
(assert (not (< low@27 high@28)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(assert (not (and (< low@27 high@28) (implies (< low@27 high@28) (= index@29 (- 0 1))))))
(check-sat)
; unknown
; [exec]
; res := index
; [eval] -1 <= res && res < |xs|
; [eval] -1 <= res
; [eval] -1
; [eval] -1 <= res ==> res < |xs|
; [eval] -1 <= res
; [eval] -1
(push) ; 4
(push) ; 5
(assert (not (not (<= (- 0 1) index@29))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= (- 0 1) index@29)))
(check-sat)
; unsat
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 75] -1 <= index@29
(assert (<= (- 0 1) index@29))
; [eval] res < |xs|
; [eval] |xs|
(pop) ; 5
; [dead else-branch 75] !-1 <= index@29
(pop) ; 4
; Joined path conditions
; [eval] 0 <= res ==> xs[res] == key
; [eval] 0 <= res
(push) ; 4
(push) ; 5
(assert (not (not (<= 0 index@29))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (<= 0 index@29)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 76] 0 <= index@29
(assert (<= 0 index@29))
; [eval] xs[res] == key
; [eval] xs[res]
(pop) ; 5
(push) ; 5
; [else-branch 76] !0 <= index@29
(assert (not (<= 0 index@29)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
; [eval] -1 == res ==> (forall i2: Int :: 0 <= i2 && i2 < |xs| ==> xs[i2] != key)
; [eval] -1 == res
; [eval] -1
(push) ; 4
(push) ; 5
(assert (not (not (= (- 0 1) index@29))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (= (- 0 1) index@29)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 77] -1 == index@29
(assert (= (- 0 1) index@29))
; [eval] (forall i2: Int :: 0 <= i2 && i2 < |xs| ==> xs[i2] != key)
(declare-const i2@40 Int)
(push) ; 6
; [eval] 0 <= i2 && i2 < |xs| ==> xs[i2] != key
; [eval] 0 <= i2 && i2 < |xs|
; [eval] 0 <= i2
; [eval] 0 <= i2 ==> i2 < |xs|
; [eval] 0 <= i2
(push) ; 7
(push) ; 8
(assert (not (not (<= 0 i2@40))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
(assert (not (<= 0 i2@40)))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
; [then-branch 78] 0 <= i2@40
(assert (<= 0 i2@40))
; [eval] i2 < |xs|
; [eval] |xs|
(pop) ; 8
(push) ; 8
; [else-branch 78] !0 <= i2@40
(assert (not (<= 0 i2@40)))
(pop) ; 8
(pop) ; 7
; Joined path conditions
; Joined path conditions
(push) ; 7
(push) ; 8
(assert (not (not (and (<= 0 i2@40) (implies (<= 0 i2@40) (< i2@40 ($Seq.length xs@15)))))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
(assert (not (and (<= 0 i2@40) (implies (<= 0 i2@40) (< i2@40 ($Seq.length xs@15))))))
(check-sat)
; unknown
(pop) ; 8
; 0,00s
; (get-info :all-statistics)
(push) ; 8
; [then-branch 79] 0 <= i2@40 && 0 <= i2@40 ==> i2@40 < |xs@15|
(assert (and (<= 0 i2@40) (implies (<= 0 i2@40) (< i2@40 ($Seq.length xs@15)))))
; [eval] xs[i2] != key
; [eval] xs[i2]
(pop) ; 8
(push) ; 8
; [else-branch 79] !0 <= i2@40 && 0 <= i2@40 ==> i2@40 < |xs@15|
(assert (not (and (<= 0 i2@40) (implies (<= 0 i2@40) (< i2@40 ($Seq.length xs@15))))))
(pop) ; 8
(pop) ; 7
; Joined path conditions
; Joined path conditions
(pop) ; 6
; Nested auxiliary terms
(pop) ; 5
(push) ; 5
; [else-branch 77] -1 != index@29
(assert (not (= (- 0 1) index@29)))
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(set-option :timeout 0)
(push) ; 4
(assert (not (implies
  (= (- 0 1) index@29)
  (forall ((i2@40 Int)) (!
    (implies
      (and (<= 0 i2@40) (implies (<= 0 i2@40) (< i2@40 ($Seq.length xs@15))))
      (not (= ($Seq.index xs@15 i2@40) key@16)))
    :pattern (($Seq.index xs@15 i2@40))
    :qid |prog.l33|)))))
(check-sat)
; unsat
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
(assert (implies
  (= (- 0 1) index@29)
  (forall ((i2@40 Int)) (!
    (implies
      (and (<= 0 i2@40) (implies (<= 0 i2@40) (< i2@40 ($Seq.length xs@15))))
      (not (= ($Seq.index xs@15 i2@40) key@16)))
    :pattern (($Seq.index xs@15 i2@40))
    :qid |prog.l33|))))
(pop) ; 3
(pop) ; 2
(pop) ; 1
