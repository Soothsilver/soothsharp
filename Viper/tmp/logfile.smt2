(get-info :version)
; (:version "4.4.0")
; Input file is D:\TEMP\tmp9FE6.tmp
; Started: 2017-05-22 21:14:28
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
(declare-fun Node_CanReachNode ($Snap $Ref $Ref $Ref) Bool)
(declare-fun Node_CanReachNode%limited ($Snap $Ref $Ref $Ref) Bool)
(declare-fun Node_CanReachNode%stateless ($Ref $Ref $Ref) Bool)
(declare-fun arrayRead ($Snap $Ref Int) Int)
(declare-fun arrayRead%limited ($Snap $Ref Int) Int)
(declare-fun arrayRead%stateless ($Ref Int) Bool)
; Snapshot variable to be used during function verification
(declare-fun s@$ () $Snap)
; Declaring predicate trigger functions
(declare-fun Node_AccessAllTree%trigger ($Snap $Ref $Ref) Bool)
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
; ---------- FUNCTION Node_CanReachNode----------
(declare-fun this@0 () $Ref)
(declare-fun target@1 () $Ref)
(declare-fun finalStop@2 () $Ref)
(declare-fun result@3 () Bool)
; ----- Well-definedness of specifications -----
(push) ; 1
(pop) ; 1
(assert (forall ((s@$ $Snap) (this@0 $Ref) (target@1 $Ref) (finalStop@2 $Ref)) (!
  (=
    (Node_CanReachNode%limited s@$ this@0 target@1 finalStop@2)
    (Node_CanReachNode s@$ this@0 target@1 finalStop@2))
  :pattern ((Node_CanReachNode s@$ this@0 target@1 finalStop@2))
  )))
(assert (forall ((s@$ $Snap) (this@0 $Ref) (target@1 $Ref) (finalStop@2 $Ref)) (!
  (Node_CanReachNode%stateless this@0 target@1 finalStop@2)
  :pattern ((Node_CanReachNode%limited s@$ this@0 target@1 finalStop@2))
  )))
; ---------- FUNCTION Node_CanReachNode (verify) ----------
(push) ; 1
; [eval] (unfolding acc(Node_AccessAllTree(this, finalStop), write) in (this == target ? true : (this == finalStop ? false : Node_CanReachNode(this.Node_Right, target, finalStop) || Node_CanReachNode(this.Node_Bottom, target, finalStop))))
(push) ; 2
(assert (Node_AccessAllTree%trigger s@$ this@0 finalStop@2))
; [eval] this != finalStop
(set-option :timeout 250)
(push) ; 3
(assert (not (= this@0 finalStop@2)))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
(assert (not (not (= this@0 finalStop@2))))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(push) ; 3
; [then-branch 0] this@0 != finalStop@2
(assert (not (= this@0 finalStop@2)))
(assert (not (= this@0 $Ref.null)))
(push) ; 4
(assert (not (=
  ($SortWrappers.$SnapTo$Ref ($Snap.first s@$))
  ($SortWrappers.$SnapTo$Ref ($Snap.first ($Snap.second s@$))))))
(check-sat)
; unknown
(pop) ; 4
; 0,00s
; (get-info :all-statistics)
; [eval] (this == target ? true : (this == finalStop ? false : Node_CanReachNode(this.Node_Right, target, finalStop) || Node_CanReachNode(this.Node_Bottom, target, finalStop)))
; [eval] this == target
(push) ; 4
(push) ; 5
(assert (not (not (= this@0 target@1))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (= this@0 target@1)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 1] this@0 == target@1
(assert (= this@0 target@1))
(pop) ; 5
(push) ; 5
; [else-branch 1] this@0 != target@1
(assert (not (= this@0 target@1)))
; [eval] (this == finalStop ? false : Node_CanReachNode(this.Node_Right, target, finalStop) || Node_CanReachNode(this.Node_Bottom, target, finalStop))
; [eval] this == finalStop
(push) ; 6
; [dead then-branch 2] this@0 == finalStop@2
(push) ; 7
; [else-branch 2] this@0 != finalStop@2
; [eval] Node_CanReachNode(this.Node_Right, target, finalStop) || Node_CanReachNode(this.Node_Bottom, target, finalStop)
; [eval] Node_CanReachNode(this.Node_Right, target, finalStop)
(push) ; 8
(pop) ; 8
; Joined path conditions
; [eval] !Node_CanReachNode(this.Node_Right, target, finalStop) && Node_CanReachNode(this.Node_Bottom, target, finalStop)
; [eval] !Node_CanReachNode(this.Node_Right, target, finalStop)
; [eval] Node_CanReachNode(this.Node_Right, target, finalStop)
(push) ; 8
(pop) ; 8
; Joined path conditions
; [eval] !Node_CanReachNode(this.Node_Right, target, finalStop) ==> Node_CanReachNode(this.Node_Bottom, target, finalStop)
; [eval] !Node_CanReachNode(this.Node_Right, target, finalStop)
; [eval] Node_CanReachNode(this.Node_Right, target, finalStop)
(push) ; 8
(pop) ; 8
; Joined path conditions
(push) ; 8
(push) ; 9
(assert (not (Node_CanReachNode ($Snap.first ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first s@$)) target@1 finalStop@2)))
(check-sat)
; unknown
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
(assert (not (not
  (Node_CanReachNode ($Snap.first ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first s@$)) target@1 finalStop@2))))
(check-sat)
; unknown
(pop) ; 9
; 0,00s
; (get-info :all-statistics)
(push) ; 9
; [then-branch 3] !Node_CanReachNode(First:Second:Second:s@$, First:s@$, target@1, finalStop@2)
(assert (not
  (Node_CanReachNode ($Snap.first ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first s@$)) target@1 finalStop@2)))
; [eval] Node_CanReachNode(this.Node_Bottom, target, finalStop)
(push) ; 10
(pop) ; 10
; Joined path conditions
(pop) ; 9
(push) ; 9
; [else-branch 3] Node_CanReachNode(First:Second:Second:s@$, First:s@$, target@1, finalStop@2)
(assert (Node_CanReachNode ($Snap.first ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first s@$)) target@1 finalStop@2))
(pop) ; 9
(pop) ; 8
; Joined path conditions
; Joined path conditions
(pop) ; 7
(pop) ; 6
; Joined path conditions
(declare-fun $deadThen@7 ($Snap $Ref $Ref $Ref) Bool)
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(pop) ; 3
(push) ; 3
; [else-branch 0] this@0 == finalStop@2
(assert (= this@0 finalStop@2))
(assert (= s@$ $Snap.unit))
; [eval] (this == target ? true : (this == finalStop ? false : Node_CanReachNode(this.Node_Right, target, finalStop) || Node_CanReachNode(this.Node_Bottom, target, finalStop)))
; [eval] this == target
(push) ; 4
(push) ; 5
(assert (not (not (= this@0 target@1))))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
(assert (not (= this@0 target@1)))
(check-sat)
; unknown
(pop) ; 5
; 0,00s
; (get-info :all-statistics)
(push) ; 5
; [then-branch 4] this@0 == target@1
(assert (= this@0 target@1))
(pop) ; 5
(push) ; 5
; [else-branch 4] this@0 != target@1
(assert (not (= this@0 target@1)))
; [eval] (this == finalStop ? false : Node_CanReachNode(this.Node_Right, target, finalStop) || Node_CanReachNode(this.Node_Bottom, target, finalStop))
; [eval] this == finalStop
(push) ; 6
(push) ; 7
(assert (not (not (= this@0 finalStop@2))))
(check-sat)
; unknown
(pop) ; 7
; 0,00s
; (get-info :all-statistics)
(push) ; 7
; [then-branch 5] this@0 == finalStop@2
(pop) ; 7
; [dead else-branch 5] this@0 != finalStop@2
(pop) ; 6
; Joined path conditions
(declare-fun $deadElse@8 ($Snap $Ref $Ref $Ref) Bool)
(pop) ; 5
(pop) ; 4
; Joined path conditions
; Joined path conditions
(pop) ; 3
(pop) ; 2
; Joined path conditions
(assert (Node_AccessAllTree%trigger s@$ this@0 finalStop@2))
(assert (implies
  (not (= this@0 finalStop@2))
  (and (not (= this@0 $Ref.null)) (not (= this@0 finalStop@2)))))
; Joined path conditions
(assert (implies (= this@0 finalStop@2) (and (= s@$ $Snap.unit) (= this@0 finalStop@2))))
(declare-fun joinedIn@9 ($Snap $Ref $Ref $Ref) Bool)
(assert (implies
  (= this@0 finalStop@2)
  (=
    (joinedIn@9 s@$ this@0 target@1 finalStop@2)
    (ite
      (= this@0 target@1)
      true
      (ite
        (= this@0 finalStop@2)
        false
        ($deadElse@8 s@$ this@0 target@1 finalStop@2))))))
(assert (implies
  (not (= this@0 finalStop@2))
  (=
    (joinedIn@9 s@$ this@0 target@1 finalStop@2)
    (ite
      (= this@0 target@1)
      true
      (ite
        (= this@0 finalStop@2)
        ($deadThen@7 s@$ this@0 target@1 finalStop@2)
        (or
          (Node_CanReachNode ($Snap.first ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first s@$)) target@1 finalStop@2)
          (and
            (not
              (Node_CanReachNode ($Snap.first ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first s@$)) target@1 finalStop@2))
            (implies
              (not
                (Node_CanReachNode ($Snap.first ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first s@$)) target@1 finalStop@2))
              (Node_CanReachNode ($Snap.second ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first ($Snap.second s@$))) target@1 finalStop@2)))))))))
(assert (= result@3 (joinedIn@9 s@$ this@0 target@1 finalStop@2)))
(pop) ; 1
(assert (and
  (forall ((s@$ $Snap) (this@0 $Ref) (target@1 $Ref) (finalStop@2 $Ref)) (!
    (let ((result@3 (Node_CanReachNode%limited s@$ this@0 target@1 finalStop@2))) (=
      (Node_CanReachNode s@$ this@0 target@1 finalStop@2)
      (ite
        (= this@0 target@1)
        true
        (ite
          (= this@0 finalStop@2)
          false
          (or
            (Node_CanReachNode%limited ($Snap.first ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first s@$)) target@1 finalStop@2)
            (Node_CanReachNode%limited ($Snap.second ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first ($Snap.second s@$))) target@1 finalStop@2))))))
    :pattern ((Node_CanReachNode s@$ this@0 target@1 finalStop@2))
    ))
  (forall ((s@$ $Snap) (this@0 $Ref) (target@1 $Ref) (finalStop@2 $Ref)) (!
    (let ((result@3 (Node_CanReachNode%limited s@$ this@0 target@1 finalStop@2))) (=
      (Node_CanReachNode s@$ this@0 target@1 finalStop@2)
      (ite
        (= this@0 target@1)
        true
        (ite
          (= this@0 finalStop@2)
          false
          (or
            (Node_CanReachNode%limited ($Snap.first ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first s@$)) target@1 finalStop@2)
            (Node_CanReachNode%limited ($Snap.second ($Snap.second ($Snap.second s@$))) ($SortWrappers.$SnapTo$Ref ($Snap.first ($Snap.second s@$))) target@1 finalStop@2))))))
    :pattern ((Node_CanReachNode%stateless this@0 target@1 finalStop@2) (Node_AccessAllTree%trigger s@$ this@0 finalStop@2))
    ))))
; ---------- FUNCTION arrayRead----------
(declare-fun array@4 () $Ref)
(declare-fun index@5 () Int)
(declare-fun result@6 () Int)
; ----- Well-definedness of specifications -----
(push) ; 1
(declare-const $k@10 $Perm)
(assert ($Perm.isReadVar $k@10 $Perm.Write))
(assert (<= $Perm.No $k@10))
(assert (implies (< $Perm.No $k@10) (not (= array@4 $Ref.null))))
(assert (= ($Snap.second s@$) $Snap.unit))
; [eval] |array.arrayContents| > index
; [eval] |array.arrayContents|
(set-option :timeout 0)
(push) ; 2
(assert (not (not (= $k@10 $Perm.No))))
(check-sat)
; unsat
(pop) ; 2
; 0,00s
; (get-info :all-statistics)
(assert (not (= $k@10 $Perm.No)))
(assert (> ($Seq.length ($SortWrappers.$SnapTo$Seq<Int> ($Snap.first s@$))) index@5))
(pop) ; 1
(assert (forall ((s@$ $Snap) (array@4 $Ref) (index@5 Int)) (!
  (= (arrayRead%limited s@$ array@4 index@5) (arrayRead s@$ array@4 index@5))
  :pattern ((arrayRead s@$ array@4 index@5))
  )))
(assert (forall ((s@$ $Snap) (array@4 $Ref) (index@5 Int)) (!
  (arrayRead%stateless array@4 index@5)
  :pattern ((arrayRead%limited s@$ array@4 index@5))
  )))
; ---------- FUNCTION arrayRead (verify) ----------
(push) ; 1
(assert (> ($Seq.length ($SortWrappers.$SnapTo$Seq<Int> ($Snap.first s@$))) index@5))
(assert (not (= $k@10 $Perm.No)))
(assert (= ($Snap.second s@$) $Snap.unit))
(assert (implies (< $Perm.No $k@10) (not (= array@4 $Ref.null))))
(assert (<= $Perm.No $k@10))
(assert ($Perm.isReadVar $k@10 $Perm.Write))
; [eval] array.arrayContents[index]
(assert (=
  result@6
  ($Seq.index ($SortWrappers.$SnapTo$Seq<Int> ($Snap.first s@$)) index@5)))
(pop) ; 1
(assert (forall ((s@$ $Snap) (array@4 $Ref) (index@5 Int)) (!
  (let ((result@6 (arrayRead%limited s@$ array@4 index@5))) (implies
    (> ($Seq.length ($SortWrappers.$SnapTo$Seq<Int> ($Snap.first s@$))) index@5)
    (=
      (arrayRead s@$ array@4 index@5)
      ($Seq.index ($SortWrappers.$SnapTo$Seq<Int> ($Snap.first s@$)) index@5))))
  :pattern ((arrayRead s@$ array@4 index@5))
  )))
; ---------- Node_AccessAllTree ----------
(declare-const self@11 $Ref)
(declare-const end2@12 $Ref)
(push) ; 1
(pop) ; 1
(push) ; 1
; [eval] self != end2
(set-option :timeout 250)
(push) ; 2
(assert (not (= self@11 end2@12)))
(check-sat)
; unknown
(pop) ; 2
; 0,00s
; (get-info :all-statistics)
(push) ; 2
(assert (not (not (= self@11 end2@12))))
(check-sat)
; unknown
(pop) ; 2
; 0,00s
; (get-info :all-statistics)
(push) ; 2
; [then-branch 6] self@11 != end2@12
(assert (not (= self@11 end2@12)))
(declare-const $t@13 $Snap)
(declare-const $t@14 $Ref)
(declare-const $t@15 $Snap)
(assert (= $t@13 ($Snap.combine ($SortWrappers.$RefTo$Snap $t@14) $t@15)))
(assert (not (= self@11 $Ref.null)))
(declare-const $t@16 $Ref)
(declare-const $t@17 $Snap)
(assert (= $t@15 ($Snap.combine ($SortWrappers.$RefTo$Snap $t@16) $t@17)))
(declare-const $t@18 $Snap)
(declare-const $t@19 $Snap)
(assert (= $t@17 ($Snap.combine $t@18 $t@19)))
(push) ; 3
(assert (not (= $t@14 $t@16)))
(check-sat)
; unknown
(pop) ; 3
; 0,00s
; (get-info :all-statistics)
(pop) ; 2
(push) ; 2
; [else-branch 6] self@11 == end2@12
(assert (= self@11 end2@12))
(declare-const $t@20 $Snap)
(assert (= $t@20 $Snap.unit))
(pop) ; 2
(pop) ; 1
; ---------- Node_init ----------
(declare-const this@21 $Ref)
(push) ; 1
(push) ; 2
(declare-const $t@22 $Snap)
(declare-const $t@23 $Ref)
(declare-const $t@24 $Ref)
(assert (=
  $t@22
  ($Snap.combine
    ($SortWrappers.$RefTo$Snap $t@23)
    ($SortWrappers.$RefTo$Snap $t@24))))
(assert (not (= this@21 $Ref.null)))
(pop) ; 2
(push) ; 2
; [exec]
; this := new(Node_Right, Node_Bottom, arrayContents)
(declare-const this@25 $Ref)
(assert (not (= this@25 $Ref.null)))
(declare-const Node_Right@26 $Ref)
(declare-const Node_Bottom@27 $Ref)
(declare-const arrayContents@28 $Seq<Int>)
(assert (and (not (= this@25 Node_Right@26)) (not (= this@25 Node_Bottom@27))))
(pop) ; 2
(pop) ; 1
