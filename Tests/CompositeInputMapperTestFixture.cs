using System;

namespace Logos.Input.Tests
{
    [TestFixture, TestOf(typeof(CompositeInputMapper))]
    public sealed class CompositeInputMapperTestFixture
    {
        [Test]
        public void Constructor_defaults_to_disabled()
        {
            CompositeInputMapper mapper = new CompositeInputMapper();

            Assert.That(mapper.IsEnabled, Is.False);
        }

        [Test]
        public void Constructor_with_initial_state_sets_enabled_state()
        {
            CompositeInputMapper mapper = new CompositeInputMapper(isEnabled: true);

            Assert.That(mapper.IsEnabled, Is.True);
        }

        [Test]
        public void IsEnabled_when_changed_updates_child_mappers_and_raises_event()
        {
            CompositeInputMapper mapper = new CompositeInputMapper();
            FakeInputMapper firstChild = new FakeInputMapper();
            FakeInputMapper secondChild = new FakeInputMapper();
            bool? enabledChangedValue = null;
            int enabledChangedCallCount = 0;

            mapper.Add(firstChild);
            mapper.Add(secondChild);
            mapper.EnabledChanged += (_, isEnabled) =>
            {
                enabledChangedValue = isEnabled;
                enabledChangedCallCount++;
            };

            mapper.IsEnabled = true;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(mapper.IsEnabled, Is.True);
                Assert.That(firstChild.IsEnabled, Is.True);
                Assert.That(secondChild.IsEnabled, Is.True);
                Assert.That(firstChild.SetCallCount, Is.EqualTo(1));
                Assert.That(secondChild.SetCallCount, Is.EqualTo(1));
                Assert.That(enabledChangedValue, Is.True);
                Assert.That(enabledChangedCallCount, Is.EqualTo(1));
            }
        }

        [Test]
        public void IsEnabled_when_value_is_unchanged_does_not_update_children_or_raise_event()
        {
            CompositeInputMapper mapper = new CompositeInputMapper(isEnabled: true);
            FakeInputMapper child = new FakeInputMapper(isEnabled: true);
            int enabledChangedCallCount = 0;

            mapper.Add(child);
            mapper.EnabledChanged += (_, _) => enabledChangedCallCount++;

            mapper.IsEnabled = true;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(child.SetCallCount, Is.EqualTo(0));
                Assert.That(enabledChangedCallCount, Is.EqualTo(0));
            }
        }

        private sealed class FakeInputMapper : IInputMapper
        {
            private bool _isEnabled;

            public FakeInputMapper()
            {
            }

            public FakeInputMapper(bool isEnabled)
            {
                _isEnabled = isEnabled;
            }

            public int SetCallCount { get; private set; }

            public bool IsEnabled
            {
                get => _isEnabled;
                set
                {
                    _isEnabled = value;
                    SetCallCount++;
                    EnabledChanged?.Invoke(this, value);
                }
            }

            public event EventHandler<bool>? EnabledChanged;
        }
    }
}
